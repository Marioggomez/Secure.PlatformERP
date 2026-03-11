namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.flujo_restablecimiento_clave.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FlujoRestablecimientoClaveDto
{
    /// <summary>
    /// Columna id_flujo_restablecimiento_clave.
    /// </summary>
    public Guid? IdFlujoRestablecimientoClave { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tipo_verificacion_restablecimiento.
    /// </summary>
    public short? IdTipoVerificacionRestablecimiento { get; set; }
    /// <summary>
    /// Columna verificacion_completada.
    /// </summary>
    public bool? VerificacionCompletada { get; set; }
    /// <summary>
    /// Columna expira_en_utc.
    /// </summary>
    public DateTime? ExpiraEnUtc { get; set; }
    /// <summary>
    /// Columna usado.
    /// </summary>
    public bool? Usado { get; set; }
    /// <summary>
    /// Columna ip_origen.
    /// </summary>
    public string? IpOrigen { get; set; }
    /// <summary>
    /// Columna agente_usuario.
    /// </summary>
    public string? AgenteUsuario { get; set; }
    /// <summary>
    /// Columna solicitud_id.
    /// </summary>
    public string? SolicitudId { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
