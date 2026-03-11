namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.modulo_permiso.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ModuloPermisoDto
{
    /// <summary>
    /// Columna id_modulo_permiso.
    /// </summary>
    public long? IdModuloPermiso { get; set; }
    /// <summary>
    /// Columna id_modulo.
    /// </summary>
    public int? IdModulo { get; set; }
    /// <summary>
    /// Columna id_permiso.
    /// </summary>
    public long? IdPermiso { get; set; }
}
