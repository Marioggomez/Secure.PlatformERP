using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.auditoria_autorizacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IAuditoriaAutorizacionRepository
{
    Task<IReadOnlyList<AuditoriaAutorizacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AuditoriaAutorizacionDto?> ObtenerAsync(long idAuditoriaAutorizacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(AuditoriaAutorizacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AuditoriaAutorizacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAuditoriaAutorizacion, string? usuario, CancellationToken cancellationToken);
}
