namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.notificacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class NotificacionDto
{
    /// <summary>
    /// Columna id_notificacion.
    /// </summary>
    public long? IdNotificacion { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna titulo.
    /// </summary>
    public string? Titulo { get; set; }
    /// <summary>
    /// Columna mensaje.
    /// </summary>
    public string? Mensaje { get; set; }
    /// <summary>
    /// Columna leida.
    /// </summary>
    public bool? Leida { get; set; }
    /// <summary>
    /// Columna fecha_creacion.
    /// </summary>
    public DateTime? FechaCreacion { get; set; }
}
