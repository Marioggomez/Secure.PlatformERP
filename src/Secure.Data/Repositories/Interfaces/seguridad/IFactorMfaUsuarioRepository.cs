using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.factor_mfa_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IFactorMfaUsuarioRepository
{
    Task<IReadOnlyList<FactorMfaUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<FactorMfaUsuarioDto?> ObtenerAsync(long idFactorMfaUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(FactorMfaUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(FactorMfaUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idFactorMfaUsuario, string? usuario, CancellationToken cancellationToken);
}
