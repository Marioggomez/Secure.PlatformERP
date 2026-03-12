namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.tipo_persona.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TipoPersonaDto
{
    /// <summary>
    /// Columna id_tipo_persona.
    /// </summary>
    public int? IdTipoPersona { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
}
