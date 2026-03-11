using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.historial_clave_usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface IHistorialClaveUsuarioRepository
{
    Task<IReadOnlyList<HistorialClaveUsuarioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<HistorialClaveUsuarioDto?> ObtenerAsync(long idHistorialClaveUsuario, CancellationToken cancellationToken);
    Task<long> CrearAsync(HistorialClaveUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(HistorialClaveUsuarioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idHistorialClaveUsuario, string? usuario, CancellationToken cancellationToken);
}
