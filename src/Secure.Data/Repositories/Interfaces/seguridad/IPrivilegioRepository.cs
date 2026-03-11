using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.privilegio.
/// Autor: Mario Gomez.
/// </summary>
public interface IPrivilegioRepository
{
    Task<IReadOnlyList<PrivilegioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PrivilegioDto?> ObtenerAsync(long idPrivilegio, CancellationToken cancellationToken);
    Task<long> CrearAsync(PrivilegioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PrivilegioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPrivilegio, string? usuario, CancellationToken cancellationToken);
}
