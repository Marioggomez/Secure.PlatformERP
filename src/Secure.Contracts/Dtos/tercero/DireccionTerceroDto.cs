namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.direccion_tercero.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DireccionTerceroDto
{
    /// <summary>
    /// Columna id_direccion_tercero.
    /// </summary>
    public long? IdDireccionTercero { get; set; }
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna id_tipo_direccion.
    /// </summary>
    public int? IdTipoDireccion { get; set; }
    /// <summary>
    /// Columna direccion_linea1.
    /// </summary>
    public string? DireccionLinea1 { get; set; }
    /// <summary>
    /// Columna direccion_linea2.
    /// </summary>
    public string? DireccionLinea2 { get; set; }
    /// <summary>
    /// Columna id_pais.
    /// </summary>
    public int? IdPais { get; set; }
    /// <summary>
    /// Columna id_estado.
    /// </summary>
    public int? IdEstado { get; set; }
    /// <summary>
    /// Columna id_ciudad.
    /// </summary>
    public int? IdCiudad { get; set; }
    /// <summary>
    /// Columna codigo_postal.
    /// </summary>
    public string? CodigoPostal { get; set; }
    /// <summary>
    /// Columna principal.
    /// </summary>
    public bool? Principal { get; set; }
}
