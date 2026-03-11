using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.estado_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IEstadoAprobacionRepository
{
    Task<IReadOnlyList<EstadoAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EstadoAprobacionDto?> ObtenerAsync(short idEstadoAprobacion, CancellationToken cancellationToken);
    Task<short> CrearAsync(EstadoAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EstadoAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idEstadoAprobacion, string? usuario, CancellationToken cancellationToken);
}
