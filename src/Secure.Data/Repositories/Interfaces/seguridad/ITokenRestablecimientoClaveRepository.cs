using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.token_restablecimiento_clave.
/// Autor: Mario Gomez.
/// </summary>
public interface ITokenRestablecimientoClaveRepository
{
    Task<IReadOnlyList<TokenRestablecimientoClaveDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TokenRestablecimientoClaveDto?> ObtenerAsync(Guid idTokenRestablecimientoClave, CancellationToken cancellationToken);
    Task<Guid> CrearAsync(TokenRestablecimientoClaveDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TokenRestablecimientoClaveDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(Guid idTokenRestablecimientoClave, string? usuario, CancellationToken cancellationToken);
}
