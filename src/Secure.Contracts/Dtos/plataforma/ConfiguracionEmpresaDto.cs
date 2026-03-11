namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.configuracion_empresa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ConfiguracionEmpresaDto
{
    /// <summary>
    /// Columna id_configuracion_empresa.
    /// </summary>
    public long? IdConfiguracionEmpresa { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_parametro_configuracion.
    /// </summary>
    public int? IdParametroConfiguracion { get; set; }
    /// <summary>
    /// Columna valor.
    /// </summary>
    public string? Valor { get; set; }
    /// <summary>
    /// Columna fecha_creacion.
    /// </summary>
    public DateTime? FechaCreacion { get; set; }
}
