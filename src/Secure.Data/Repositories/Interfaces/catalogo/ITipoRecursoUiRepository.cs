using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_recurso_ui.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoRecursoUiRepository
{
    Task<IReadOnlyList<TipoRecursoUiDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoRecursoUiDto?> ObtenerAsync(short idTipoRecursoUi, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoRecursoUiDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoRecursoUiDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoRecursoUi, string? usuario, CancellationToken cancellationToken);
}
