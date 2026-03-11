using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.tenant.
/// Autor: Mario Gomez.
/// </summary>
public interface ITenantRepository
{
    Task<IReadOnlyList<TenantDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TenantDto?> ObtenerAsync(long idTenant, CancellationToken cancellationToken);
    Task<long> CrearAsync(TenantDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TenantDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idTenant, string? usuario, CancellationToken cancellationToken);
}
