namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.contacto_tercero.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ContactoTerceroDto
{
    /// <summary>
    /// Columna id_contacto_tercero.
    /// </summary>
    public long? IdContactoTercero { get; set; }
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna id_tipo_contacto.
    /// </summary>
    public int? IdTipoContacto { get; set; }
    /// <summary>
    /// Columna valor.
    /// </summary>
    public string? Valor { get; set; }
    /// <summary>
    /// Columna principal.
    /// </summary>
    public bool? Principal { get; set; }
}
