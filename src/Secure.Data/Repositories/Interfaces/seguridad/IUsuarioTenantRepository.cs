using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_tenant.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioTenantRepository
{
    Task<IReadOnlyList<UsuarioTenantDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioTenantDto?> ObtenerAsync(long idUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioTenantDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioTenantDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuario, string? usuario, CancellationToken cancellationToken);
}
