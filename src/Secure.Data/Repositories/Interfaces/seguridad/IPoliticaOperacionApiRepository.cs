using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.politica_operacion_api.
/// Autor: Mario Gomez.
/// </summary>
public interface IPoliticaOperacionApiRepository
{
    Task<IReadOnlyList<PoliticaOperacionApiDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PoliticaOperacionApiDto?> ObtenerAsync(long idPoliticaOperacionApi, CancellationToken cancellationToken);
    Task<long> CrearAsync(PoliticaOperacionApiDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PoliticaOperacionApiDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPoliticaOperacionApi, string? usuario, CancellationToken cancellationToken);
}
