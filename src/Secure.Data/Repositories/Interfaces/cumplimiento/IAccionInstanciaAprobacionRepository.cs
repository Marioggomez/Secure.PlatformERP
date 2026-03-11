using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.accion_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IAccionInstanciaAprobacionRepository
{
    Task<IReadOnlyList<AccionInstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AccionInstanciaAprobacionDto?> ObtenerAsync(long idAccionInstanciaAprobacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAccionInstanciaAprobacion, string? usuario, CancellationToken cancellationToken);
}
