namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.modulo.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ModuloDto
{
    /// <summary>
    /// Columna id_modulo.
    /// </summary>
    public int? IdModulo { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna orden.
    /// </summary>
    public int? Orden { get; set; }
}
