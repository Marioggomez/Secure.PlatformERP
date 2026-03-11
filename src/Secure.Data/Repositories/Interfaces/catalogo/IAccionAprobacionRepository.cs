using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.accion_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IAccionAprobacionRepository
{
    Task<IReadOnlyList<AccionAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AccionAprobacionDto?> ObtenerAsync(short idAccionAprobacion, CancellationToken cancellationToken);
    Task<short> CrearAsync(AccionAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AccionAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idAccionAprobacion, string? usuario, CancellationToken cancellationToken);
}
