using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.paso_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IPasoInstanciaAprobacionRepository
{
    Task<IReadOnlyList<PasoInstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PasoInstanciaAprobacionDto?> ObtenerAsync(long idPasoInstanciaAprobacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPasoInstanciaAprobacion, string? usuario, CancellationToken cancellationToken);
}
