namespace Secure.Platform.Infrastructure.Observability;

/// <summary>
/// Contrato de auditoria y errores tecnicos de API.
/// Autor: Mario Gomez.
/// </summary>
public interface IApiObservabilityService
{
    /// <summary>
    /// Registra una operacion API en observabilidad.operacion_api_log.
    /// </summary>
    Task LogOperationAsync(
        Guid? correlationId,
        string? endpoint,
        string? metodoHttp,
        string? usuario,
        int? codigoHttp,
        int? duracionMs,
        string? ip,
        long? idTenant,
        CancellationToken cancellationToken);

    /// <summary>
    /// Registra un error tecnico en observabilidad.error_log.
    /// </summary>
    Task LogErrorAsync(
        Guid? correlationId,
        string? usuario,
        string? endpoint,
        string? mensajeError,
        string? stackTrace,
        string? payload,
        CancellationToken cancellationToken);
}
