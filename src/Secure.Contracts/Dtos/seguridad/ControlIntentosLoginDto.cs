namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.control_intentos_login.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ControlIntentosLoginDto
{
    /// <summary>
    /// Columna id_control_intento.
    /// </summary>
    public long? IdControlIntento { get; set; }
    /// <summary>
    /// Columna login_usuario.
    /// </summary>
    public string? LoginUsuario { get; set; }
    /// <summary>
    /// Columna ip.
    /// </summary>
    public string? Ip { get; set; }
    /// <summary>
    /// Columna intentos.
    /// </summary>
    public int? Intentos { get; set; }
    /// <summary>
    /// Columna fecha_ultimo_intento.
    /// </summary>
    public DateTime? FechaUltimoIntento { get; set; }
    /// <summary>
    /// Columna bloqueado_hasta.
    /// </summary>
    public DateTime? BloqueadoHasta { get; set; }
}
