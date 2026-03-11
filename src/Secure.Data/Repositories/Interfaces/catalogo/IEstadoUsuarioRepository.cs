using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.estado_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IEstadoUsuarioRepository
{
    Task<IReadOnlyList<EstadoUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EstadoUsuarioDto?> ObtenerAsync(short idEstadoUsuario, CancellationToken cancellationToken);
    Task<short> CrearAsync(EstadoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EstadoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idEstadoUsuario, string? usuario, CancellationToken cancellationToken);
}
