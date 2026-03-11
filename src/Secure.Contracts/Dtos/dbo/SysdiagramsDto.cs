namespace Secure.Platform.Contracts.Dtos.Dbo;

/// <summary>
/// DTO de la tabla dbo.sysdiagrams.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SysdiagramsDto
{
    /// <summary>
    /// Columna name.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Columna principal_id.
    /// </summary>
    public int? PrincipalId { get; set; }
    /// <summary>
    /// Columna diagram_id.
    /// </summary>
    public int? DiagramId { get; set; }
    /// <summary>
    /// Columna version.
    /// </summary>
    public int? Version { get; set; }
    /// <summary>
    /// Columna definition.
    /// </summary>
    public byte[]? Definition { get; set; }
}
