using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.feature_flag.
/// Autor: Mario Gomez.
/// </summary>
public interface IFeatureFlagRepository
{
    Task<IReadOnlyList<FeatureFlagDto>> ListarAsync(CancellationToken cancellationToken);
    Task<FeatureFlagDto?> ObtenerAsync(long idFeature, CancellationToken cancellationToken);
    Task<long> CrearAsync(FeatureFlagDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(FeatureFlagDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idFeature, string? usuario, CancellationToken cancellationToken);
}
