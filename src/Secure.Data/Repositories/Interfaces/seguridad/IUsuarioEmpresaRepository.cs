using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioEmpresaRepository
{
    Task<IReadOnlyList<UsuarioEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioEmpresaDto?> ObtenerAsync(long idUsuarioEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuarioEmpresa, string? usuario, CancellationToken cancellationToken);
}
