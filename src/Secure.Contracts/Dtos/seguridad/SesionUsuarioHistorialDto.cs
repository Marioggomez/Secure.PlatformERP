namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.sesion_usuario_historial.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionUsuarioHistorialDto
{
    /// <summary>
    /// Columna id_historial.
    /// </summary>
    public long? IdHistorial { get; set; }
    /// <summary>
    /// Columna id_sesion.
    /// </summary>
    public long? IdSesion { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna fecha_inicio.
    /// </summary>
    public DateTime? FechaInicio { get; set; }
    /// <summary>
    /// Columna fecha_fin.
    /// </summary>
    public DateTime? FechaFin { get; set; }
    /// <summary>
    /// Columna ip.
    /// </summary>
    public string? Ip { get; set; }
    /// <summary>
    /// Columna dispositivo.
    /// </summary>
    public string? Dispositivo { get; set; }
    /// <summary>
    /// Columna motivo_cierre.
    /// </summary>
    public string? MotivoCierre { get; set; }
}
