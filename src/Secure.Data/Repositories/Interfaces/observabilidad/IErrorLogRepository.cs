using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.error_log.
/// Autor: Mario Gomez.
/// </summary>
public interface IErrorLogRepository
{
    Task<IReadOnlyList<ErrorLogDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ErrorLogDto?> ObtenerAsync(long idErrorLog, CancellationToken cancellationToken);
    Task<long> CrearAsync(ErrorLogDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ErrorLogDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idErrorLog, string? usuario, CancellationToken cancellationToken);
}
