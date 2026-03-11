namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.categoria_configuracion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CategoriaConfiguracionDto
{
    /// <summary>
    /// Columna id_categoria_configuracion.
    /// </summary>
    public int? IdCategoriaConfiguracion { get; set; }
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
}
