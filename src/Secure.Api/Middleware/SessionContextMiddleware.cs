using System.Security.Claims;
using Secure.Platform.Data.Sql;
using Secure.Platform.Infrastructure.Security;

namespace Secure.Platform.Api.Middleware;

/// <summary>
/// Middleware para validar bearer token opaco y cargar contexto de sesion IAM.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SessionContextMiddleware
{
    private static readonly HashSet<string> EndpointsPublicos = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/v1/seguridad/flujo_autenticacion/iniciar",
        "/api/v1/seguridad/flujo_autenticacion/seleccionar_empresa",
        "/api/v1/seguridad/desafio_mfa/validar",
        "/api/v1/seguridad/desafio_mfa/reenviar",
        "/api/v1/seguridad/flujo_restablecimiento_clave/iniciar",
        "/api/v1/seguridad/flujo_restablecimiento_clave/completar"
    };

    private readonly RequestDelegate _next;

    public SessionContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ISesionContextoService sesionContextoService)
    {
        SqlScopeContext.Clear();

        if (HttpMethods.IsOptions(context.Request.Method))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (EsRutaPublica(context.Request.Path))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        var token = ExtraerBearerToken(context.Request.Headers.Authorization);
        if (string.IsNullOrWhiteSpace(token))
        {
            await ResponderNoAutorizadoAsync(context, "Debe enviar Authorization Bearer token.").ConfigureAwait(false);
            return;
        }

        var sesion = await sesionContextoService
            .ResolverSesionAsync(token, true, context.RequestAborted)
            .ConfigureAwait(false);

        if (sesion is null || !sesion.Activo)
        {
            await ResponderNoAutorizadoAsync(context, "La sesion no existe o esta inactiva.").ConfigureAwait(false);
            return;
        }

        if (!sesion.MfaValidado)
        {
            await ResponderNoAutorizadoAsync(context, "La sesion no cumple validacion MFA.").ConfigureAwait(false);
            return;
        }

        if (DateTime.UtcNow > sesion.ExpiraAbsolutaUtc)
        {
            await ResponderNoAutorizadoAsync(context, "La sesion expiro.").ConfigureAwait(false);
            return;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, sesion.IdUsuario.ToString()),
            new(ClaimTypes.Name, sesion.UsuarioMostrar ?? $"usuario:{sesion.IdUsuario}"),
            new("id_sesion_usuario", sesion.IdSesionUsuario.ToString()),
            new("id_tenant", sesion.IdTenant.ToString()),
            new("id_empresa", sesion.IdEmpresa.ToString())
        };

        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "OpaqueBearer"));
        context.Items["SessionContext"] = sesion;
        SqlScopeContext.Set(sesion.IdTenant, sesion.IdEmpresa, sesion.IdUsuario);

        try
        {
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            SqlScopeContext.Clear();
        }
    }

    private static bool EsRutaPublica(PathString path)
    {
        var value = path.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        if (value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return EndpointsPublicos.Contains(value);
    }

    private static string? ExtraerBearerToken(string? authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            return null;
        }

        const string bearerPrefix = "Bearer ";
        if (!authorizationHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var token = authorizationHeader[bearerPrefix.Length..].Trim();
        return string.IsNullOrWhiteSpace(token) ? null : token;
    }

    private static Task ResponderNoAutorizadoAsync(HttpContext context, string mensaje)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(new
        {
            autenticado = false,
            mensaje
        }, context.RequestAborted);
    }
}
