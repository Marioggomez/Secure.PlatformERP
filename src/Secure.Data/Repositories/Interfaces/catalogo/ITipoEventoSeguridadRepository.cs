using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_evento_seguridad.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoEventoSeguridadRepository
{
    Task<IReadOnlyList<TipoEventoSeguridadDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoEventoSeguridadDto?> ObtenerAsync(short idTipoEventoSeguridad, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoEventoSeguridadDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoEventoSeguridadDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoEventoSeguridad, string? usuario, CancellationToken cancellationToken);
}
