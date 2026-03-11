using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.paso_perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IPasoPerfilAprobacionRepository
{
    Task<IReadOnlyList<PasoPerfilAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PasoPerfilAprobacionDto?> ObtenerAsync(long idPasoPerfilAprobacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(PasoPerfilAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PasoPerfilAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPasoPerfilAprobacion, string? usuario, CancellationToken cancellationToken);
}
