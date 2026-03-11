using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_scope_unidad.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioScopeUnidadRepository
{
    Task<IReadOnlyList<UsuarioScopeUnidadDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioScopeUnidadDto?> ObtenerAsync(long idUsuarioScopeUnidad, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioScopeUnidadDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioScopeUnidadDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuarioScopeUnidad, string? usuario, CancellationToken cancellationToken);
}
