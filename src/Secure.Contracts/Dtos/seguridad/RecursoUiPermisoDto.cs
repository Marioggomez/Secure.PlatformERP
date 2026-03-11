namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.recurso_ui_permiso.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiPermisoDto
{
    /// <summary>
    /// Columna id_recurso_ui.
    /// </summary>
    public long? IdRecursoUi { get; set; }
    /// <summary>
    /// Columna id_permiso.
    /// </summary>
    public int? IdPermiso { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
