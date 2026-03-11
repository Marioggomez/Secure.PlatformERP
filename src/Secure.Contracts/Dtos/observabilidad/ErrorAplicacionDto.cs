namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.error_aplicacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ErrorAplicacionDto
{
    /// <summary>
    /// Columna id_error_aplicacion.
    /// </summary>
    public long? IdErrorAplicacion { get; set; }
    /// <summary>
    /// Columna fecha_utc.
    /// </summary>
    public DateTime? FechaUtc { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_sesion_usuario.
    /// </summary>
    public Guid? IdSesionUsuario { get; set; }
    /// <summary>
    /// Columna solicitud_id.
    /// </summary>
    public string? SolicitudId { get; set; }
    /// <summary>
    /// Columna endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
    /// <summary>
    /// Columna metodo_http.
    /// </summary>
    public string? MetodoHttp { get; set; }
    /// <summary>
    /// Columna query_string.
    /// </summary>
    public string? QueryString { get; set; }
    /// <summary>
    /// Columna ip_origen.
    /// </summary>
    public string? IpOrigen { get; set; }
    /// <summary>
    /// Columna agente_usuario.
    /// </summary>
    public string? AgenteUsuario { get; set; }
    /// <summary>
    /// Columna tipo_error.
    /// </summary>
    public string? TipoError { get; set; }
    /// <summary>
    /// Columna mensaje_error.
    /// </summary>
    public string? MensajeError { get; set; }
    /// <summary>
    /// Columna traza_error.
    /// </summary>
    public string? TrazaError { get; set; }
    /// <summary>
    /// Columna mensaje_interno.
    /// </summary>
    public string? MensajeInterno { get; set; }
    /// <summary>
    /// Columna traza_interna.
    /// </summary>
    public string? TrazaInterna { get; set; }
    /// <summary>
    /// Columna origen_error.
    /// </summary>
    public string? OrigenError { get; set; }
    /// <summary>
    /// Columna codigo_http.
    /// </summary>
    public int? CodigoHttp { get; set; }
}
