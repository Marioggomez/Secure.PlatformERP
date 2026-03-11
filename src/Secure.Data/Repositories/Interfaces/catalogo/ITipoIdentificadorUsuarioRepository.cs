using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_identificador_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoIdentificadorUsuarioRepository
{
    Task<IReadOnlyList<TipoIdentificadorUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoIdentificadorUsuarioDto?> ObtenerAsync(short idTipoIdentificadorUsuario, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoIdentificadorUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoIdentificadorUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoIdentificadorUsuario, string? usuario, CancellationToken cancellationToken);
}
