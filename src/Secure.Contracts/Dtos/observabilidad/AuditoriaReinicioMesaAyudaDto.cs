namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.auditoria_reinicio_mesa_ayuda.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaReinicioMesaAyudaDto
{
    /// <summary>
    /// Columna id_auditoria_reinicio_mesa_ayuda.
    /// </summary>
    public long? IdAuditoriaReinicioMesaAyuda { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_usuario_afectado.
    /// </summary>
    public long? IdUsuarioAfectado { get; set; }
    /// <summary>
    /// Columna id_usuario_administrador.
    /// </summary>
    public long? IdUsuarioAdministrador { get; set; }
    /// <summary>
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
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
