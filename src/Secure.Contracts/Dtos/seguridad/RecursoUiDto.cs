namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.recurso_ui.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiDto
{
    /// <summary>
    /// Columna id_recurso_ui.
    /// </summary>
    public long? IdRecursoUi { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna id_tipo_recurso_ui.
    /// </summary>
    public short? IdTipoRecursoUi { get; set; }
    /// <summary>
    /// Columna ruta.
    /// </summary>
    public string? Ruta { get; set; }
    /// <summary>
    /// Columna componente.
    /// </summary>
    public string? Componente { get; set; }
    /// <summary>
    /// Columna icono.
    /// </summary>
    public string? Icono { get; set; }
    /// <summary>
    /// Columna id_recurso_ui_padre.
    /// </summary>
    public long? IdRecursoUiPadre { get; set; }
    /// <summary>
    /// Columna orden_visual.
    /// </summary>
    public int? OrdenVisual { get; set; }
    /// <summary>
    /// Columna es_visible.
    /// </summary>
    public bool? EsVisible { get; set; }
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
