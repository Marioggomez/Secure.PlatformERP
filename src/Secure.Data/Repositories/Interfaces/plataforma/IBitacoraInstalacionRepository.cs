using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.bitacora_instalacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IBitacoraInstalacionRepository
{
    Task<IReadOnlyList<BitacoraInstalacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<BitacoraInstalacionDto?> ObtenerAsync(long idBitacoraInstalacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(BitacoraInstalacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(BitacoraInstalacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idBitacoraInstalacion, string? usuario, CancellationToken cancellationToken);
}
