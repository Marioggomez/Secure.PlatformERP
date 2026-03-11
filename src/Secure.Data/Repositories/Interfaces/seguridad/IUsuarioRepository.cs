using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioRepository
{
    Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PaginacionResultadoDto<UsuarioListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken);
    Task<UsuarioDto?> ObtenerAsync(long idUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuario, string? usuario, CancellationToken cancellationToken);
}
