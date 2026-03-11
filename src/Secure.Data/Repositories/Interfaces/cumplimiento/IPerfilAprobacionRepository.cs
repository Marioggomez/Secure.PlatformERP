using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IPerfilAprobacionRepository
{
    Task<IReadOnlyList<PerfilAprobacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PerfilAprobacionDto?> ObtenerAsync(long idPerfilAprobacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(PerfilAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PerfilAprobacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idPerfilAprobacion, string? usuario, CancellationToken cancellationToken);
}
