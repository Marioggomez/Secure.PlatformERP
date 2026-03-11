using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.efecto_permiso.
/// Autor: Mario Gomez.
/// </summary>
public interface IEfectoPermisoRepository
{
    Task<IReadOnlyList<EfectoPermisoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EfectoPermisoDto?> ObtenerAsync(short idEfectoPermiso, CancellationToken cancellationToken);
    Task<short> CrearAsync(EfectoPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EfectoPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idEfectoPermiso, string? usuario, CancellationToken cancellationToken);
}
