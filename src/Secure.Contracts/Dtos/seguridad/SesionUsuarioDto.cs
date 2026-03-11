namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.sesion_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionUsuarioDto
{
    /// <summary>
    /// Columna id_sesion_usuario.
    /// </summary>
    public Guid? IdSesionUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna token_hash.
    /// </summary>
    public byte[]? TokenHash { get; set; }
    /// <summary>
    /// Columna refresh_hash.
    /// </summary>
    public byte[]? RefreshHash { get; set; }
    /// <summary>
    /// Columna origen_autenticacion.
    /// </summary>
    public string? OrigenAutenticacion { get; set; }
    /// <summary>
    /// Columna mfa_validado.
    /// </summary>
    public bool? MfaValidado { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna expira_absoluta_utc.
    /// </summary>
    public DateTime? ExpiraAbsolutaUtc { get; set; }
    /// <summary>
    /// Columna ultima_actividad_utc.
    /// </summary>
    public DateTime? UltimaActividadUtc { get; set; }
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
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna revocada_utc.
    /// </summary>
    public DateTime? RevocadaUtc { get; set; }
    /// <summary>
    /// Columna motivo_revocacion.
    /// </summary>
    public string? MotivoRevocacion { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
