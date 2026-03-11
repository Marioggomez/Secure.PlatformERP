namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.desafio_mfa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DesafioMfaDto
{
    /// <summary>
    /// Columna id_desafio_mfa.
    /// </summary>
    public Guid? IdDesafioMfa { get; set; }
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
    /// Columna id_sesion_usuario.
    /// </summary>
    public Guid? IdSesionUsuario { get; set; }
    /// <summary>
    /// Columna id_flujo_autenticacion.
    /// </summary>
    public Guid? IdFlujoAutenticacion { get; set; }
    /// <summary>
    /// Columna id_proposito_desafio_mfa.
    /// </summary>
    public short? IdPropositoDesafioMfa { get; set; }
    /// <summary>
    /// Columna id_canal_notificacion.
    /// </summary>
    public short? IdCanalNotificacion { get; set; }
    /// <summary>
    /// Columna codigo_accion.
    /// </summary>
    public string? CodigoAccion { get; set; }
    /// <summary>
    /// Columna otp_hash.
    /// </summary>
    public byte[]? OtpHash { get; set; }
    /// <summary>
    /// Columna otp_salt.
    /// </summary>
    public byte[]? OtpSalt { get; set; }
    /// <summary>
    /// Columna expira_en_utc.
    /// </summary>
    public DateTime? ExpiraEnUtc { get; set; }
    /// <summary>
    /// Columna usado.
    /// </summary>
    public bool? Usado { get; set; }
    /// <summary>
    /// Columna intentos.
    /// </summary>
    public short? Intentos { get; set; }
    /// <summary>
    /// Columna max_intentos.
    /// </summary>
    public short? MaxIntentos { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna validado_utc.
    /// </summary>
    public DateTime? ValidadoUtc { get; set; }
}
