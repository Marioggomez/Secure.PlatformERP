namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.accion_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AccionInstanciaAprobacionDto
{
    /// <summary>
    /// Columna id_accion_instancia_aprobacion.
    /// </summary>
    public long? IdAccionInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna id_instancia_aprobacion.
    /// </summary>
    public long? IdInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna id_paso_instancia_aprobacion.
    /// </summary>
    public long? IdPasoInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_unidad_organizativa.
    /// </summary>
    public long? IdUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna id_accion_aprobacion.
    /// </summary>
    public short? IdAccionAprobacion { get; set; }
    /// <summary>
    /// Columna comentario.
    /// </summary>
    public string? Comentario { get; set; }
    /// <summary>
    /// Columna mfa_validado.
    /// </summary>
    public bool? MfaValidado { get; set; }
    /// <summary>
    /// Columna ip_origen.
    /// </summary>
    public string? IpOrigen { get; set; }
    /// <summary>
    /// Columna agente_usuario.
    /// </summary>
    public string? AgenteUsuario { get; set; }
    /// <summary>
    /// Columna fecha_utc.
    /// </summary>
    public DateTime? FechaUtc { get; set; }
}
