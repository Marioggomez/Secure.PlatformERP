using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.recurso_ui.
/// Autor: Mario Gomez.
/// </summary>
public interface IRecursoUiRepository
{
    Task<IReadOnlyList<RecursoUiDto>> ListarAsync(CancellationToken cancellationToken);
    Task<RecursoUiDto?> ObtenerAsync(long idRecursoUi, CancellationToken cancellationToken);
    Task<long> CrearAsync(RecursoUiDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(RecursoUiDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idRecursoUi, string? usuario, CancellationToken cancellationToken);
}
