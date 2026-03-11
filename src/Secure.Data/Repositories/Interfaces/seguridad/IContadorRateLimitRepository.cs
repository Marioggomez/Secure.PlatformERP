using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.contador_rate_limit.
/// Autor: Mario Gomez.
/// </summary>
public interface IContadorRateLimitRepository
{
    Task<IReadOnlyList<ContadorRateLimitDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ContadorRateLimitDto?> ObtenerAsync(long idContadorRateLimit, CancellationToken cancellationToken);
    Task<long> CrearAsync(ContadorRateLimitDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ContadorRateLimitDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idContadorRateLimit, string? usuario, CancellationToken cancellationToken);
}
