using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.politica_tenant.
/// Autor: Mario Gomez.
/// </summary>
public interface IPoliticaTenantRepository
{
    Task<IReadOnlyList<PoliticaTenantDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PoliticaTenantDto?> ObtenerAsync(long idTenant, CancellationToken cancellationToken);
    Task<long> CrearAsync(PoliticaTenantDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PoliticaTenantDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idTenant, string? usuario, CancellationToken cancellationToken);
}
