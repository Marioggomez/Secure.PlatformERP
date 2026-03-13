using System.Security.Cryptography;
using System.Text;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Implementacion para validar y resolver sesiones activas por token hash.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionContextoService : ISesionContextoService
{
    private readonly IIamAuthRepository _repository;

    public SesionContextoService(IIamAuthRepository repository)
    {
        _repository = repository;
    }

    public async Task<SesionContextoActual?> ResolverSesionAsync(string tokenPlano, bool actualizarActividadUtc, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(tokenPlano))
        {
            return null;
        }

        var tokenNormalizado = tokenPlano.Trim();
        if (!TryParseGuidToken(tokenNormalizado, out var tokenGuid))
        {
            return null;
        }

        var tokenHash = CalcularHashToken(tokenGuid);
        var sesion = await _repository.ObtenerSesionPorTokenHashAsync(tokenHash, actualizarActividadUtc, cancellationToken).ConfigureAwait(false);
        if (sesion is null)
        {
            return null;
        }

        return new SesionContextoActual
        {
            IdSesionUsuario = sesion.IdSesionUsuario,
            IdUsuario = sesion.IdUsuario,
            IdTenant = sesion.IdTenant,
            IdEmpresa = sesion.IdEmpresa,
            MfaValidado = sesion.MfaValidado,
            Activo = sesion.Activo,
            ExpiraAbsolutaUtc = sesion.ExpiraAbsolutaUtc,
            UsuarioMostrar = sesion.UsuarioMostrar
        };
    }

    private static bool TryParseGuidToken(string tokenPlano, out Guid token)
    {
        if (Guid.TryParseExact(tokenPlano, "N", out token))
        {
            return true;
        }

        return Guid.TryParse(tokenPlano, out token);
    }

    private static byte[] CalcularHashToken(Guid token)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(token.ToString("N")));
    }
}
