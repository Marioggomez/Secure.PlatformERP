namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.error_log.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ErrorLogDto
{
    /// <summary>
    /// Columna id_error_log.
    /// </summary>
    public long? IdErrorLog { get; set; }
    /// <summary>
    /// Columna correlation_id.
    /// </summary>
    public Guid? CorrelationId { get; set; }
    /// <summary>
    /// Columna usuario.
    /// </summary>
    public string? Usuario { get; set; }
    /// <summary>
    /// Columna endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
    /// <summary>
    /// Columna mensaje_error.
    /// </summary>
    public string? MensajeError { get; set; }
    /// <summary>
    /// Columna stacktrace.
    /// </summary>
    public string? Stacktrace { get; set; }
    /// <summary>
    /// Columna payload.
    /// </summary>
    public string? Payload { get; set; }
    /// <summary>
    /// Columna fecha.
    /// </summary>
    public DateTime? Fecha { get; set; }
}
