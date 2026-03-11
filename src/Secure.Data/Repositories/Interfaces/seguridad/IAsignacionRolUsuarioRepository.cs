using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.asignacion_rol_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IAsignacionRolUsuarioRepository
{
    Task<IReadOnlyList<AsignacionRolUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AsignacionRolUsuarioDto?> ObtenerAsync(long idAsignacionRolUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(AsignacionRolUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AsignacionRolUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAsignacionRolUsuario, string? usuario, CancellationToken cancellationToken);
}
