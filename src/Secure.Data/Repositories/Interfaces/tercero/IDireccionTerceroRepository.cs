using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.direccion_tercero.
/// Autor: Mario Gomez.
/// </summary>
public interface IDireccionTerceroRepository
{
    Task<IReadOnlyList<DireccionTerceroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<DireccionTerceroDto?> ObtenerAsync(long idDireccionTercero, CancellationToken cancellationToken);
    Task<long> CrearAsync(DireccionTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(DireccionTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idDireccionTercero, string? usuario, CancellationToken cancellationToken);
}
