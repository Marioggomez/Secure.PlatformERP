using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.filtro_dato_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IFiltroDatoUsuarioRepository
{
    Task<IReadOnlyList<FiltroDatoUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<FiltroDatoUsuarioDto?> ObtenerAsync(long idFiltroDatoUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(FiltroDatoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(FiltroDatoUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idFiltroDatoUsuario, string? usuario, CancellationToken cancellationToken);
}
