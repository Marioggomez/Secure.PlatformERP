namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.identificacion_tercero.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IdentificacionTerceroDto
{
    /// <summary>
    /// Columna id_identificacion_tercero.
    /// </summary>
    public long? IdIdentificacionTercero { get; set; }
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna id_tipo_identificacion.
    /// </summary>
    public int? IdTipoIdentificacion { get; set; }
    /// <summary>
    /// Columna numero_identificacion.
    /// </summary>
    public string? NumeroIdentificacion { get; set; }
    /// <summary>
    /// Columna fecha_emision.
    /// </summary>
    public DateTime? FechaEmision { get; set; }
    /// <summary>
    /// Columna fecha_vencimiento.
    /// </summary>
    public DateTime? FechaVencimiento { get; set; }
    /// <summary>
    /// Columna principal.
    /// </summary>
    public bool? Principal { get; set; }
}
