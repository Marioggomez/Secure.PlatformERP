namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.excepcion_sod.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ExcepcionSodDto
{
    /// <summary>
    /// Columna id_excepcion_sod.
    /// </summary>
    public long? IdExcepcionSod { get; set; }
    /// <summary>
    /// Columna id_regla_sod.
    /// </summary>
    public long? IdReglaSod { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna fecha_inicio_utc.
    /// </summary>
    public DateTime? FechaInicioUtc { get; set; }
    /// <summary>
    /// Columna fecha_fin_utc.
    /// </summary>
    public DateTime? FechaFinUtc { get; set; }
    /// <summary>
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
    /// <summary>
    /// Columna aprobado_por.
    /// </summary>
    public long? AprobadoPor { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
