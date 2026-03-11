using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.job_sistema.
/// Autor: Mario Gomez.
/// </summary>
public interface IJobSistemaRepository
{
    Task<IReadOnlyList<JobSistemaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<JobSistemaDto?> ObtenerAsync(long idJob, CancellationToken cancellationToken);
    Task<long> CrearAsync(JobSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(JobSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idJob, string? usuario, CancellationToken cancellationToken);
}
