namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioDto
{
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna login_principal.
    /// </summary>
    public string? LoginPrincipal { get; set; }
    /// <summary>
    /// Columna login_normalizado.
    /// </summary>
    public string? LoginNormalizado { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna apellido.
    /// </summary>
    public string? Apellido { get; set; }
    /// <summary>
    /// Columna nombre_mostrar.
    /// </summary>
    public string? NombreMostrar { get; set; }
    /// <summary>
    /// Columna correo_electronico.
    /// </summary>
    public string? CorreoElectronico { get; set; }
    /// <summary>
    /// Columna correo_normalizado.
    /// </summary>
    public string? CorreoNormalizado { get; set; }
    /// <summary>
    /// Columna telefono_movil.
    /// </summary>
    public string? TelefonoMovil { get; set; }
    /// <summary>
    /// Columna idioma.
    /// </summary>
    public string? Idioma { get; set; }
    /// <summary>
    /// Columna zona_horaria.
    /// </summary>
    public string? ZonaHoraria { get; set; }
    /// <summary>
    /// Columna id_estado_usuario.
    /// </summary>
    public short? IdEstadoUsuario { get; set; }
    /// <summary>
    /// Columna bloqueado_hasta_utc.
    /// </summary>
    public DateTime? BloqueadoHastaUtc { get; set; }
    /// <summary>
    /// Columna mfa_habilitado.
    /// </summary>
    public bool? MfaHabilitado { get; set; }
    /// <summary>
    /// Columna requiere_cambio_clave.
    /// </summary>
    public bool? RequiereCambioClave { get; set; }
    /// <summary>
    /// Columna ultimo_acceso_utc.
    /// </summary>
    public DateTime? UltimoAccesoUtc { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_por.
    /// </summary>
    public long? CreadoPor { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_por.
    /// </summary>
    public long? ActualizadoPor { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
