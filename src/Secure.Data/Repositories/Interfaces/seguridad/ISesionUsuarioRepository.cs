using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.sesion_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface ISesionUsuarioRepository
{
    Task<IReadOnlyList<SesionUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<SesionUsuarioDto?> ObtenerAsync(Guid idSesionUsuario, CancellationToken cancellationToken);
    Task<Guid> CrearAsync(SesionUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(SesionUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(Guid idSesionUsuario, string? usuario, CancellationToken cancellationToken);
}
