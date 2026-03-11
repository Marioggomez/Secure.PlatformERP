using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.dispositivo_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IDispositivoUsuarioRepository
{
    Task<IReadOnlyList<DispositivoUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<DispositivoUsuarioDto?> ObtenerAsync(long idDispositivoUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(DispositivoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(DispositivoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idDispositivoUsuario, string? usuario, CancellationToken cancellationToken);
}
