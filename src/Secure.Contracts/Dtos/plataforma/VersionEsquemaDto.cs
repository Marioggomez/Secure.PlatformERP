namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.version_esquema.
/// Autor: Mario Gomez.
/// </summary>
public sealed class VersionEsquemaDto
{
    /// <summary>
    /// Columna id_version_esquema.
    /// </summary>
    public long? IdVersionEsquema { get; set; }
    /// <summary>
    /// Columna componente.
    /// </summary>
    public string? Componente { get; set; }
    /// <summary>
    /// Columna version_codigo.
    /// </summary>
    public string? VersionCodigo { get; set; }
    /// <summary>
    /// Columna checksum.
    /// </summary>
    public string? Checksum { get; set; }
    /// <summary>
    /// Columna instalado_por.
    /// </summary>
    public string? InstaladoPor { get; set; }
    /// <summary>
    /// Columna instalado_utc.
    /// </summary>
    public DateTime? InstaladoUtc { get; set; }
    /// <summary>
    /// Columna notas.
    /// </summary>
    public string? Notas { get; set; }
}
