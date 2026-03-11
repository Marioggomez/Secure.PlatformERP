using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.excepcion_permiso_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IExcepcionPermisoUsuarioRepository
{
    Task<IReadOnlyList<ExcepcionPermisoUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ExcepcionPermisoUsuarioDto?> ObtenerAsync(long idExcepcionPermisoUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idExcepcionPermisoUsuario, string? usuario, CancellationToken cancellationToken);
}
