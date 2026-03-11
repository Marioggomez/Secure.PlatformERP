using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.tenant_feature.
/// Autor: Mario Gomez.
/// </summary>
public interface ITenantFeatureRepository
{
    Task<IReadOnlyList<TenantFeatureDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TenantFeatureDto?> ObtenerAsync(long idTenantFeature, CancellationToken cancellationToken);
    Task<long> CrearAsync(TenantFeatureDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TenantFeatureDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idTenantFeature, string? usuario, CancellationToken cancellationToken);
}
