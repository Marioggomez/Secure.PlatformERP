namespace Secure.Platform.Contracts.Dtos.Catalogo;

/// <summary>
/// DTO de la tabla catalogo.canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CanalNotificacionDto
{
    /// <summary>
    /// Columna id_canal_notificacion.
    /// </summary>
    public short? IdCanalNotificacion { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna orden_visual.
    /// </summary>
    public short? OrdenVisual { get; set; }
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
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
