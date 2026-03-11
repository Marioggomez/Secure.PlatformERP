namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.permiso.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PermisoDto
{
    /// <summary>
    /// Columna id_permiso.
    /// </summary>
    public int? IdPermiso { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna modulo.
    /// </summary>
    public string? Modulo { get; set; }
    /// <summary>
    /// Columna accion.
    /// </summary>
    public string? Accion { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna es_sensible.
    /// </summary>
    public bool? EsSensible { get; set; }
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
