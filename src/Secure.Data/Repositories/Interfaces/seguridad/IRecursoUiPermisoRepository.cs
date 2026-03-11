using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.recurso_ui_permiso.
/// Autor: Mario Gomez.
/// </summary>
public interface IRecursoUiPermisoRepository
{
    Task<IReadOnlyList<RecursoUiPermisoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<RecursoUiPermisoDto?> ObtenerAsync(long idRecursoUi, CancellationToken cancellationToken);
    Task<long> CrearAsync(RecursoUiPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(RecursoUiPermisoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idRecursoUi, string? usuario, CancellationToken cancellationToken);
}
