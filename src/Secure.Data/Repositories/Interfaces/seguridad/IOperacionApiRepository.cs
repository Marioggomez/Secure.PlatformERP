using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.operacion_api.
/// Autor: Mario Gomez.
/// </summary>
public interface IOperacionApiRepository
{
    Task<IReadOnlyList<OperacionApiDto>> ListarAsync(CancellationToken cancellationToken);
    Task<OperacionApiDto?> ObtenerAsync(long idOperacionApi, CancellationToken cancellationToken);
    Task<long> CrearAsync(OperacionApiDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(OperacionApiDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idOperacionApi, string? usuario, CancellationToken cancellationToken);
}
