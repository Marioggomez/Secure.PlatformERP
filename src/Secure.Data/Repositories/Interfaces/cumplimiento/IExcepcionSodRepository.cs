using Secure.Platform.Contracts.Dtos.Cumplimiento;

namespace Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

/// <summary>
/// Contrato del repositorio para cumplimiento.excepcion_sod.
/// Autor: Mario Gomez.
/// </summary>
public interface IExcepcionSodRepository
{
    Task<IReadOnlyList<ExcepcionSodDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ExcepcionSodDto?> ObtenerAsync(long idExcepcionSod, CancellationToken cancellationToken);
    Task<long> CrearAsync(ExcepcionSodDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ExcepcionSodDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idExcepcionSod, string? usuario, CancellationToken cancellationToken);
}
