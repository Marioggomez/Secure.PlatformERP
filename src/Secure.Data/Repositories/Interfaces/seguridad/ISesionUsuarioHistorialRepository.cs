using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.sesion_usuario_historial.
/// Autor: Mario Gomez.
/// </summary>
public interface ISesionUsuarioHistorialRepository
{
    Task<IReadOnlyList<SesionUsuarioHistorialDto>> ListarAsync(CancellationToken cancellationToken);
    Task<SesionUsuarioHistorialDto?> ObtenerAsync(long idHistorial, CancellationToken cancellationToken);
    Task<long> CrearAsync(SesionUsuarioHistorialDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(SesionUsuarioHistorialDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idHistorial, string? usuario, CancellationToken cancellationToken);
}
