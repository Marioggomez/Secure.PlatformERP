using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.rol_deber.
/// Autor: Mario Gomez.
/// </summary>
public interface IRolDeberRepository
{
    Task<IReadOnlyList<RolDeberDto>> ListarAsync(CancellationToken cancellationToken);
    Task<RolDeberDto?> ObtenerAsync(long idRol, CancellationToken cancellationToken);
    Task<long> CrearAsync(RolDeberDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(RolDeberDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idRol, string? usuario, CancellationToken cancellationToken);
}
