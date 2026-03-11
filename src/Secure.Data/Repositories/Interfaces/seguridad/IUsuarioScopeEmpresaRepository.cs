using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_scope_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioScopeEmpresaRepository
{
    Task<IReadOnlyList<UsuarioScopeEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioScopeEmpresaDto?> ObtenerAsync(long idUsuarioScopeEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuarioScopeEmpresa, string? usuario, CancellationToken cancellationToken);
}
