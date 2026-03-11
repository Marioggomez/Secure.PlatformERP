using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
public interface ICanalNotificacionRepository
{
    Task<IReadOnlyList<CanalNotificacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<CanalNotificacionDto?> ObtenerAsync(short idCanalNotificacion, CancellationToken cancellationToken);
    Task<short> CrearAsync(CanalNotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(CanalNotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idCanalNotificacion, string? usuario, CancellationToken cancellationToken);
}
