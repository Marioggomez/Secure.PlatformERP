namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.regla_sod.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ReglaSodDto
{
    /// <summary>
    /// Columna id_regla_sod.
    /// </summary>
    public long? IdReglaSod { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_permiso_a.
    /// </summary>
    public int? IdPermisoA { get; set; }
    /// <summary>
    /// Columna id_permiso_b.
    /// </summary>
    public int? IdPermisoB { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna prohibe_mismo_usuario.
    /// </summary>
    public bool? ProhibeMismoUsuario { get; set; }
    /// <summary>
    /// Columna prohibe_misma_unidad.
    /// </summary>
    public bool? ProhibeMismaUnidad { get; set; }
    /// <summary>
    /// Columna prohibe_misma_sesion.
    /// </summary>
    public bool? ProhibeMismaSesion { get; set; }
    /// <summary>
    /// Columna prohibe_mismo_dia.
    /// </summary>
    public bool? ProhibeMismoDia { get; set; }
    /// <summary>
    /// Columna id_severidad_sod.
    /// </summary>
    public short? IdSeveridadSod { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
}
