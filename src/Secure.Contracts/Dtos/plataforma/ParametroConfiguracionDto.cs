namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.parametro_configuracion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ParametroConfiguracionDto
{
    /// <summary>
    /// Columna id_parametro_configuracion.
    /// </summary>
    public int? IdParametroConfiguracion { get; set; }
    /// <summary>
    /// Columna id_categoria_configuracion.
    /// </summary>
    public int? IdCategoriaConfiguracion { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna tipo_valor.
    /// </summary>
    public string? TipoValor { get; set; }
    /// <summary>
    /// Columna valor_defecto.
    /// </summary>
    public string? ValorDefecto { get; set; }
}
