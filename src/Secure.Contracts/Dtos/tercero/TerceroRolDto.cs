namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.tercero_rol.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TerceroRolDto
{
    /// <summary>
    /// Columna id_tercero_rol.
    /// </summary>
    public long? IdTerceroRol { get; set; }
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna id_rol_tercero.
    /// </summary>
    public int? IdRolTercero { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
