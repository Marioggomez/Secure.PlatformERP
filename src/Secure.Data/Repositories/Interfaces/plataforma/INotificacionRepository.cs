using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.notificacion.
/// Autor: Mario Gomez.
/// </summary>
public interface INotificacionRepository
{
    Task<IReadOnlyList<NotificacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<NotificacionDto?> ObtenerAsync(long idNotificacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(NotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(NotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idNotificacion, string? usuario, CancellationToken cancellationToken);
}
