using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.deber.
/// Autor: Mario Gomez.
/// </summary>
public interface IDeberRepository
{
    Task<IReadOnlyList<DeberDto>> ListarAsync(CancellationToken cancellationToken);
    Task<DeberDto?> ObtenerAsync(long idDeber, CancellationToken cancellationToken);
    Task<long> CrearAsync(DeberDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(DeberDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idDeber, string? usuario, CancellationToken cancellationToken);
}
