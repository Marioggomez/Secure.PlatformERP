namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO de la tabla organizacion.relacion_empresa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RelacionEmpresaDto
{
    /// <summary>
    /// Columna id_relacion_empresa.
    /// </summary>
    public long? IdRelacionEmpresa { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa_origen.
    /// </summary>
    public long? IdEmpresaOrigen { get; set; }
    /// <summary>
    /// Columna id_empresa_destino.
    /// </summary>
    public long? IdEmpresaDestino { get; set; }
    /// <summary>
    /// Columna id_tipo_relacion_empresa.
    /// </summary>
    public short? IdTipoRelacionEmpresa { get; set; }
    /// <summary>
    /// Columna fecha_inicio_utc.
    /// </summary>
    public DateTime? FechaInicioUtc { get; set; }
    /// <summary>
    /// Columna fecha_fin_utc.
    /// </summary>
    public DateTime? FechaFinUtc { get; set; }
    /// <summary>
    /// Columna observacion.
    /// </summary>
    public string? Observacion { get; set; }
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
