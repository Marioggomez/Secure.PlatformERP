using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.modulo_permiso.
/// Autor: Mario Gomez.
/// </summary>
public interface IModuloPermisoRepository
{
    Task<IReadOnlyList<ModuloPermisoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ModuloPermisoDto?> ObtenerAsync(long idModuloPermiso, CancellationToken cancellationToken);
    Task<long> CrearAsync(ModuloPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ModuloPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idModuloPermiso, string? usuario, CancellationToken cancellationToken);
}
