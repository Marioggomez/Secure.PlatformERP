using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.privilegio_permiso.
/// Autor: Mario Gomez.
/// </summary>
public interface IPrivilegioPermisoRepository
{
    Task<IReadOnlyList<PrivilegioPermisoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PrivilegioPermisoDto?> ObtenerAsync(long idPrivilegio, CancellationToken cancellationToken);
    Task<long> CrearAsync(PrivilegioPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PrivilegioPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPrivilegio, string? usuario, CancellationToken cancellationToken);
}
