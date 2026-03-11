namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.job_sistema.
/// Autor: Mario Gomez.
/// </summary>
public sealed class JobSistemaDto
{
    /// <summary>
    /// Columna id_job.
    /// </summary>
    public long? IdJob { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna cron.
    /// </summary>
    public string? Cron { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
