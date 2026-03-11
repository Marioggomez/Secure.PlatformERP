namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.job_sistema_ejecucion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class JobSistemaEjecucionDto
{
    /// <summary>
    /// Columna id_ejecucion.
    /// </summary>
    public long? IdEjecucion { get; set; }
    /// <summary>
    /// Columna id_job.
    /// </summary>
    public long? IdJob { get; set; }
    /// <summary>
    /// Columna fecha_inicio.
    /// </summary>
    public DateTime? FechaInicio { get; set; }
    /// <summary>
    /// Columna fecha_fin.
    /// </summary>
    public DateTime? FechaFin { get; set; }
    /// <summary>
    /// Columna estado.
    /// </summary>
    public string? Estado { get; set; }
    /// <summary>
    /// Columna mensaje.
    /// </summary>
    public string? Mensaje { get; set; }
}
