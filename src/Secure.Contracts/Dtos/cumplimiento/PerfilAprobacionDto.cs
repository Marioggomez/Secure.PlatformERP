namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PerfilAprobacionDto
{
    /// <summary>
    /// Columna id_perfil_aprobacion.
    /// </summary>
    public long? IdPerfilAprobacion { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna tipo_proceso.
    /// </summary>
    public string? TipoProceso { get; set; }
    /// <summary>
    /// Columna requiere_mfa.
    /// </summary>
    public bool? RequiereMfa { get; set; }
    /// <summary>
    /// Columna impide_autoaprobacion.
    /// </summary>
    public bool? ImpideAutoaprobacion { get; set; }
    /// <summary>
    /// Columna impide_misma_unidad.
    /// </summary>
    public bool? ImpideMismaUnidad { get; set; }
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
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
