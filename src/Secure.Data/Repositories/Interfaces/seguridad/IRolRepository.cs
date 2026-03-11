using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.rol.
/// Autor: Mario Gomez.
/// </summary>
public interface IRolRepository
{
    Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken cancellationToken);
    Task<RolDto?> ObtenerAsync(long idRol, CancellationToken cancellationToken);
    Task<long> CrearAsync(RolDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(RolDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idRol, string? usuario, CancellationToken cancellationToken);
}
