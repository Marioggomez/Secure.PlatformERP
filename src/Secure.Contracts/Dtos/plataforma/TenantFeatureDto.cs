namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.tenant_feature.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TenantFeatureDto
{
    /// <summary>
    /// Columna id_tenant_feature.
    /// </summary>
    public long? IdTenantFeature { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_feature.
    /// </summary>
    public long? IdFeature { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
