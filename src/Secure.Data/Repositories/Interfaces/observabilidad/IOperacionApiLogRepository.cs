using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.operacion_api_log.
/// Autor: Mario Gomez.
/// </summary>
public interface IOperacionApiLogRepository
{
    Task<IReadOnlyList<OperacionApiLogDto>> ListarAsync(CancellationToken cancellationToken);
    Task<OperacionApiLogDto?> ObtenerAsync(long idOperacionApiLog, CancellationToken cancellationToken);
    Task<long> CrearAsync(OperacionApiLogDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(OperacionApiLogDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idOperacionApiLog, string? usuario, CancellationToken cancellationToken);
}
