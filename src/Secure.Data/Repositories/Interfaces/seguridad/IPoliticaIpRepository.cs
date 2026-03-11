using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.politica_ip.
/// Autor: Mario Gomez.
/// </summary>
public interface IPoliticaIpRepository
{
    Task<IReadOnlyList<PoliticaIpDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PoliticaIpDto?> ObtenerAsync(long idPoliticaIp, CancellationToken cancellationToken);
    Task<long> CrearAsync(PoliticaIpDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PoliticaIpDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPoliticaIp, string? usuario, CancellationToken cancellationToken);
}
