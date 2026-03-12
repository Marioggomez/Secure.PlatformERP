using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.tercero.
/// Autor: Mario Gomez.
/// </summary>
public interface ITerceroRepository
{
    Task<IReadOnlyList<TerceroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PaginacionResultadoDto<TerceroListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken);
    Task<TerceroDto?> ObtenerAsync(long idTercero, CancellationToken cancellationToken);
    Task<long> CrearAsync(TerceroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TerceroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idTercero, string? usuario, CancellationToken cancellationToken);
}
