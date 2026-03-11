using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.auditoria_evento_seguridad.
/// Autor: Mario Gomez.
/// </summary>
public interface IAuditoriaEventoSeguridadRepository
{
    Task<IReadOnlyList<AuditoriaEventoSeguridadDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AuditoriaEventoSeguridadDto?> ObtenerAsync(long idAuditoriaEventoSeguridad, CancellationToken cancellationToken);
    Task<long> CrearAsync(AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAuditoriaEventoSeguridad, string? usuario, CancellationToken cancellationToken);
}
