namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.configuracion_canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ConfiguracionCanalNotificacionDto
{
    /// <summary>
    /// Columna id_configuracion_canal_notificacion.
    /// </summary>
    public long? IdConfiguracionCanalNotificacion { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_canal_notificacion.
    /// </summary>
    public short? IdCanalNotificacion { get; set; }
    /// <summary>
    /// Columna host.
    /// </summary>
    public string? Host { get; set; }
    /// <summary>
    /// Columna puerto.
    /// </summary>
    public int? Puerto { get; set; }
    /// <summary>
    /// Columna usa_ssl.
    /// </summary>
    public bool? UsaSsl { get; set; }
    /// <summary>
    /// Columna usuario_tecnico.
    /// </summary>
    public string? UsuarioTecnico { get; set; }
    /// <summary>
    /// Columna referencia_secreto.
    /// </summary>
    public string? ReferenciaSecreto { get; set; }
    /// <summary>
    /// Columna secreto_cifrado.
    /// </summary>
    public byte[]? SecretoCifrado { get; set; }
    /// <summary>
    /// Columna remitente_correo.
    /// </summary>
    public string? RemitenteCorreo { get; set; }
    /// <summary>
    /// Columna nombre_remitente.
    /// </summary>
    public string? NombreRemitente { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
}
