using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_verificacion_restablecimiento.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoVerificacionRestablecimientoRepository
{
    Task<IReadOnlyList<TipoVerificacionRestablecimientoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoVerificacionRestablecimientoDto?> ObtenerAsync(short idTipoVerificacionRestablecimiento, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoVerificacionRestablecimientoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoVerificacionRestablecimientoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoVerificacionRestablecimiento, string? usuario, CancellationToken cancellationToken);
}
