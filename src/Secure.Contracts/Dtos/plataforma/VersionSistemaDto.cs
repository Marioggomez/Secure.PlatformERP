namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.version_sistema.
/// Autor: Mario Gomez.
/// </summary>
public sealed class VersionSistemaDto
{
    /// <summary>
    /// Columna id_version_sistema.
    /// </summary>
    public int? IdVersionSistema { get; set; }
    /// <summary>
    /// Columna version.
    /// </summary>
    public string? Version { get; set; }
    /// <summary>
    /// Columna fecha_lanzamiento.
    /// </summary>
    public DateTime? FechaLanzamiento { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
}
