using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Secure.Platform.Api.Middleware;

/// <summary>
/// Middleware para reforzar consistencia de scope tenant/empresa en requests de negocio.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ScopeEnforcementMiddleware
{
    private static readonly string[] TenantKeys = ["id_tenant", "idtenant"];
    private static readonly string[] EmpresaKeys = ["id_empresa", "idempresa"];

    private readonly RequestDelegate _next;

    public ScopeEnforcementMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (HttpMethods.IsOptions(context.Request.Method) || !RequiereScope(context.Request.Path))
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            await _next(context).ConfigureAwait(false);
            return;
        }

        var idTenantSesion = ParseLongClaim(context.User, "id_tenant");
        var idEmpresaSesion = ParseLongClaim(context.User, "id_empresa");
        if (!idTenantSesion.HasValue || !idEmpresaSesion.HasValue)
        {
            await ResponderScopeInvalidoAsync(context, "No existe contexto de tenant/empresa en la sesion.").ConfigureAwait(false);
            return;
        }

        if (TieneConflictoEnRoute(context.Request.RouteValues, TenantKeys, idTenantSesion.Value, out var tenantRouteValor))
        {
            await ResponderScopeInvalidoAsync(context, $"El id_tenant de la solicitud ({tenantRouteValor}) no coincide con la sesion.").ConfigureAwait(false);
            return;
        }

        if (TieneConflictoEnRoute(context.Request.RouteValues, EmpresaKeys, idEmpresaSesion.Value, out var empresaRouteValor))
        {
            await ResponderScopeInvalidoAsync(context, $"El id_empresa de la solicitud ({empresaRouteValor}) no coincide con la sesion.").ConfigureAwait(false);
            return;
        }

        if (TieneConflictoEnQuery(context.Request.Query, TenantKeys, idTenantSesion.Value, out var tenantQueryValor))
        {
            await ResponderScopeInvalidoAsync(context, $"El id_tenant del query ({tenantQueryValor}) no coincide con la sesion.").ConfigureAwait(false);
            return;
        }

        if (TieneConflictoEnQuery(context.Request.Query, EmpresaKeys, idEmpresaSesion.Value, out var empresaQueryValor))
        {
            await ResponderScopeInvalidoAsync(context, $"El id_empresa del query ({empresaQueryValor}) no coincide con la sesion.").ConfigureAwait(false);
            return;
        }

        if (HttpMethods.IsPost(context.Request.Method) ||
            HttpMethods.IsPut(context.Request.Method) ||
            HttpMethods.IsPatch(context.Request.Method))
        {
            var payload = await ReadPayloadAsync(context.Request).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(payload))
            {
                if (TieneConflictoEnJson(payload!, TenantKeys, idTenantSesion.Value, out var tenantBodyValor))
                {
                    await ResponderScopeInvalidoAsync(context, $"El id_tenant del payload ({tenantBodyValor}) no coincide con la sesion.").ConfigureAwait(false);
                    return;
                }

                if (TieneConflictoEnJson(payload!, EmpresaKeys, idEmpresaSesion.Value, out var empresaBodyValor))
                {
                    await ResponderScopeInvalidoAsync(context, $"El id_empresa del payload ({empresaBodyValor}) no coincide con la sesion.").ConfigureAwait(false);
                    return;
                }
            }
        }

        await _next(context).ConfigureAwait(false);
    }

    private static bool RequiereScope(PathString path)
    {
        var value = (path.Value ?? string.Empty).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (!value.StartsWith("/api/v1/", StringComparison.Ordinal))
        {
            return false;
        }

        return
            !value.StartsWith("/api/v1/seguridad/flujo_autenticacion", StringComparison.Ordinal) &&
            !value.StartsWith("/api/v1/seguridad/desafio_mfa", StringComparison.Ordinal) &&
            !value.StartsWith("/api/v1/seguridad/flujo_restablecimiento_clave", StringComparison.Ordinal);
    }

    private static long? ParseLongClaim(ClaimsPrincipal user, string claimType)
    {
        var raw = user.FindFirst(claimType)?.Value;
        return long.TryParse(raw, out var parsed) ? parsed : null;
    }

    private static bool TieneConflictoEnRoute(
        RouteValueDictionary routeValues,
        IEnumerable<string> keys,
        long expectedValue,
        out string conflictingValue)
    {
        foreach (var key in keys)
        {
            foreach (var routeKey in routeValues.Keys)
            {
                if (!string.Equals(routeKey.Replace("_", string.Empty), key.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var routeValue = routeValues[routeKey]?.ToString();
                if (string.IsNullOrWhiteSpace(routeValue))
                {
                    continue;
                }

                if (long.TryParse(routeValue, out var parsedValue) && parsedValue != expectedValue)
                {
                    conflictingValue = routeValue;
                    return true;
                }
            }
        }

        conflictingValue = string.Empty;
        return false;
    }

    private static bool TieneConflictoEnQuery(
        IQueryCollection query,
        IEnumerable<string> keys,
        long expectedValue,
        out string conflictingValue)
    {
        foreach (var entry in query)
        {
            var normalizedKey = entry.Key.Replace("_", string.Empty);
            if (!keys.Any(k => string.Equals(normalizedKey, k.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            var raw = entry.Value.ToString();
            if (string.IsNullOrWhiteSpace(raw))
            {
                continue;
            }

            if (long.TryParse(raw, out var parsedValue) && parsedValue != expectedValue)
            {
                conflictingValue = raw;
                return true;
            }
        }

        conflictingValue = string.Empty;
        return false;
    }

    private static bool TieneConflictoEnJson(
        string payload,
        IEnumerable<string> keys,
        long expectedValue,
        out string conflictingValue)
    {
        using var document = JsonDocument.Parse(payload);
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            conflictingValue = string.Empty;
            return false;
        }

        foreach (var property in document.RootElement.EnumerateObject())
        {
            var normalizedName = property.Name.Replace("_", string.Empty);
            if (!keys.Any(k => string.Equals(normalizedName, k.Replace("_", string.Empty), StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            long parsedValue;
            if (property.Value.ValueKind == JsonValueKind.Number && property.Value.TryGetInt64(out parsedValue))
            {
                if (parsedValue != expectedValue)
                {
                    conflictingValue = parsedValue.ToString();
                    return true;
                }
            }
            else if (property.Value.ValueKind == JsonValueKind.String &&
                     long.TryParse(property.Value.GetString(), out parsedValue) &&
                     parsedValue != expectedValue)
            {
                conflictingValue = parsedValue.ToString();
                return true;
            }
        }

        conflictingValue = string.Empty;
        return false;
    }

    private static async Task<string?> ReadPayloadAsync(HttpRequest request)
    {
        if (request.Body is null || !request.Body.CanRead)
        {
            return null;
        }

        if (request.ContentLength.GetValueOrDefault() <= 0)
        {
            return null;
        }

        request.EnableBuffering();
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var payload = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;

        return payload;
    }

    private static Task ResponderScopeInvalidoAsync(HttpContext context, string mensaje)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(new
        {
            autorizado = false,
            mensaje
        }, context.RequestAborted);
    }
}
