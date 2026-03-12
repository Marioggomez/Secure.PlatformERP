using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.tercero_rol.
/// Autor: Mario Gomez.
/// </summary>
public interface ITerceroRolRepository
{
    Task<IReadOnlyList<TerceroRolDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TerceroRolDto?> ObtenerAsync(long idTerceroRol, CancellationToken cancellationToken);
    Task<long> CrearAsync(TerceroRolDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TerceroRolDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idTerceroRol, string? usuario, CancellationToken cancellationToken);
}
