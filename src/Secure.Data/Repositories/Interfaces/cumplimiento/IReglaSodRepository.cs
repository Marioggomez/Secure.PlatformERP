using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.regla_sod.
/// Autor: Mario Gomez.
/// </summary>
public interface IReglaSodRepository
{
    Task<IReadOnlyList<ReglaSodDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ReglaSodDto?> ObtenerAsync(long idReglaSod, CancellationToken cancellationToken);
    Task<long> CrearAsync(ReglaSodDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ReglaSodDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idReglaSod, string? usuario, CancellationToken cancellationToken);
}
