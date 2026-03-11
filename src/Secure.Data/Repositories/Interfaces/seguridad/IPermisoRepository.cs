using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.permiso.
/// Autor: Mario Gomez.
/// </summary>
public interface IPermisoRepository
{
    Task<IReadOnlyList<PermisoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PermisoDto?> ObtenerAsync(int idPermiso, CancellationToken cancellationToken);
    Task<int> CrearAsync(PermisoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PermisoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idPermiso, string? usuario, CancellationToken cancellationToken);
}
