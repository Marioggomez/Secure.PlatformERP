using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.identificacion_tercero.
/// Autor: Mario Gomez.
/// </summary>
public interface IIdentificacionTerceroRepository
{
    Task<IReadOnlyList<IdentificacionTerceroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<IdentificacionTerceroDto?> ObtenerAsync(long idIdentificacionTercero, CancellationToken cancellationToken);
    Task<long> CrearAsync(IdentificacionTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(IdentificacionTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idIdentificacionTercero, string? usuario, CancellationToken cancellationToken);
}
