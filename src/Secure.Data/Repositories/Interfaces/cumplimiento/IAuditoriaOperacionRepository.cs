using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.auditoria_operacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IAuditoriaOperacionRepository
{
    Task<IReadOnlyList<AuditoriaOperacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AuditoriaOperacionDto?> ObtenerAsync(long idAuditoria, CancellationToken cancellationToken);
    Task<long> CrearAsync(AuditoriaOperacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AuditoriaOperacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAuditoria, string? usuario, CancellationToken cancellationToken);
}
