namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.privilegio_permiso.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PrivilegioPermisoDto
{
    /// <summary>
    /// Columna id_privilegio.
    /// </summary>
    public long? IdPrivilegio { get; set; }
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
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
}
