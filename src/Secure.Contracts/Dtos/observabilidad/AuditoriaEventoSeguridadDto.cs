namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.auditoria_evento_seguridad.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaEventoSeguridadDto
{
    /// <summary>
    /// Columna id_auditoria_evento_seguridad.
    /// </summary>
    public long? IdAuditoriaEventoSeguridad { get; set; }
    /// <summary>
    /// Columna fecha_utc.
    /// </summary>
    public DateTime? FechaUtc { get; set; }
    /// <summary>
    /// Columna id_tipo_evento_seguridad.
    /// </summary>
    public short? IdTipoEventoSeguridad { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_sesion_usuario.
    /// </summary>
    public Guid? IdSesionUsuario { get; set; }
    /// <summary>
    /// Columna detalle.
    /// </summary>
    public string? Detalle { get; set; }
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
}
