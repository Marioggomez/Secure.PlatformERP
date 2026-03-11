namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.feature_flag.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FeatureFlagDto
{
    /// <summary>
    /// Columna id_feature.
    /// </summary>
    public long? IdFeature { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
}
