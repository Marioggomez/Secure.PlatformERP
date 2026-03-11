namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.integracion_externa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IntegracionExternaDto
{
    /// <summary>
    /// Columna id_integracion.
    /// </summary>
    public long? IdIntegracion { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
