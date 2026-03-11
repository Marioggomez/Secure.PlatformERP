using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_identificador.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioIdentificadorRepository
{
    Task<IReadOnlyList<UsuarioIdentificadorDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioIdentificadorDto?> ObtenerAsync(long idUsuarioIdentificador, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioIdentificadorDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioIdentificadorDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuarioIdentificador, string? usuario, CancellationToken cancellationToken);
}
