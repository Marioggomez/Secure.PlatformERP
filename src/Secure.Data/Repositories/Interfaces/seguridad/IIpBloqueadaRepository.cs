using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.ip_bloqueada.
/// Autor: Mario Gomez.
/// </summary>
public interface IIpBloqueadaRepository
{
    Task<IReadOnlyList<IpBloqueadaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<IpBloqueadaDto?> ObtenerAsync(long idIpBloqueada, CancellationToken cancellationToken);
    Task<long> CrearAsync(IpBloqueadaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(IpBloqueadaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idIpBloqueada, string? usuario, CancellationToken cancellationToken);
}
