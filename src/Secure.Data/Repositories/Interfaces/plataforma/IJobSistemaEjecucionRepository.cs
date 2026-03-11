using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.job_sistema_ejecucion.
/// Autor: Mario Gomez.
/// </summary>
public interface IJobSistemaEjecucionRepository
{
    Task<IReadOnlyList<JobSistemaEjecucionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<JobSistemaEjecucionDto?> ObtenerAsync(long idEjecucion, CancellationToken cancellationToken);
    Task<long> CrearAsync(JobSistemaEjecucionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(JobSistemaEjecucionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idEjecucion, string? usuario, CancellationToken cancellationToken);
}
