namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.tenant.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TenantDto
{
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
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
    /// Columna dominio_principal.
    /// </summary>
    public string? DominioPrincipal { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
    /// <summary>
    /// Columna es_entrenamiento.
    /// </summary>
    public bool? EsEntrenamiento { get; set; }
}
