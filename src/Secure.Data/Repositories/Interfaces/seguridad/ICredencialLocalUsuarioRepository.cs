using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.credencial_local_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface ICredencialLocalUsuarioRepository
{
    Task<IReadOnlyList<CredencialLocalUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<CredencialLocalUsuarioDto?> ObtenerAsync(long idUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(CredencialLocalUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(CredencialLocalUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuario, string? usuario, CancellationToken cancellationToken);
}
