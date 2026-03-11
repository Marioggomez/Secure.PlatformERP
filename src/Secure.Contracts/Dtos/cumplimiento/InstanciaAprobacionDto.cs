namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class InstanciaAprobacionDto
{
    /// <summary>
    /// Columna id_instancia_aprobacion.
    /// </summary>
    public long? IdInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_unidad_organizativa.
    /// </summary>
    public long? IdUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna id_perfil_aprobacion.
    /// </summary>
    public long? IdPerfilAprobacion { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna id_objeto.
    /// </summary>
    public long? IdObjeto { get; set; }
    /// <summary>
    /// Columna nivel_actual.
    /// </summary>
    public byte? NivelActual { get; set; }
    /// <summary>
    /// Columna id_estado_aprobacion.
    /// </summary>
    public short? IdEstadoAprobacion { get; set; }
    /// <summary>
    /// Columna solicitado_por.
    /// </summary>
    public long? SolicitadoPor { get; set; }
    /// <summary>
    /// Columna solicitado_utc.
    /// </summary>
    public DateTime? SolicitadoUtc { get; set; }
    /// <summary>
    /// Columna expira_utc.
    /// </summary>
    public DateTime? ExpiraUtc { get; set; }
    /// <summary>
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
    /// <summary>
    /// Columna hash_payload.
    /// </summary>
    public byte[]? HashPayload { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
