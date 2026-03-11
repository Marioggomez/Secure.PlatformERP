using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.integracion_externa.
/// Autor: Mario Gomez.
/// </summary>
public interface IIntegracionExternaRepository
{
    Task<IReadOnlyList<IntegracionExternaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<IntegracionExternaDto?> ObtenerAsync(long idIntegracion, CancellationToken cancellationToken);
    Task<long> CrearAsync(IntegracionExternaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(IntegracionExternaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idIntegracion, string? usuario, CancellationToken cancellationToken);
}
