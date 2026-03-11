namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.operacion_api_log.
/// Autor: Mario Gomez.
/// </summary>
public sealed class OperacionApiLogDto
{
    /// <summary>
    /// Columna id_operacion_api_log.
    /// </summary>
    public long? IdOperacionApiLog { get; set; }
    /// <summary>
    /// Columna correlation_id.
    /// </summary>
    public Guid? CorrelationId { get; set; }
    /// <summary>
    /// Columna endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
    /// <summary>
    /// Columna metodo_http.
    /// </summary>
    public string? MetodoHttp { get; set; }
    /// <summary>
    /// Columna usuario.
    /// </summary>
    public string? Usuario { get; set; }
    /// <summary>
    /// Columna codigo_http.
    /// </summary>
    public int? CodigoHttp { get; set; }
    /// <summary>
    /// Columna duracion_ms.
    /// </summary>
    public int? DuracionMs { get; set; }
    /// <summary>
    /// Columna ip.
    /// </summary>
    public string? Ip { get; set; }
    /// <summary>
    /// Columna fecha.
    /// </summary>
    public DateTime? Fecha { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
}
