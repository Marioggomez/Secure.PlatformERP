using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.configuracion_canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IConfiguracionCanalNotificacionRepository
{
    Task<IReadOnlyList<ConfiguracionCanalNotificacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ConfiguracionCanalNotificacionDto?> ObtenerAsync(long idConfiguracionCanalNotificacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idConfiguracionCanalNotificacion, string? usuario, CancellationToken cancellationToken);
}
