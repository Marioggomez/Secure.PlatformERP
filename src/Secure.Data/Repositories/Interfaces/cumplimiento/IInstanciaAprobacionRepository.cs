using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IInstanciaAprobacionRepository
{
    Task<IReadOnlyList<InstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<InstanciaAprobacionDto?> ObtenerAsync(long idInstanciaAprobacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(InstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(InstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idInstanciaAprobacion, string? usuario, CancellationToken cancellationToken);
}
