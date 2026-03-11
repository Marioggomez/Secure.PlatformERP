namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.flujo_autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FlujoAutenticacionDto
{
    /// <summary>
    /// Columna id_flujo_autenticacion.
    /// </summary>
    public Guid? IdFlujoAutenticacion { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna mfa_requerido.
    /// </summary>
    public bool? MfaRequerido { get; set; }
    /// <summary>
    /// Columna mfa_validado.
    /// </summary>
    public bool? MfaValidado { get; set; }
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
    /// Columna huella_dispositivo.
    /// </summary>
    public string? HuellaDispositivo { get; set; }
    /// <summary>
    /// Columna solicitud_id.
    /// </summary>
    public string? SolicitudId { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
