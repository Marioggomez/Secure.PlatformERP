namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.auditoria_autorizacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaAutorizacionDto
{
    /// <summary>
    /// Columna id_auditoria_autorizacion.
    /// </summary>
    public long? IdAuditoriaAutorizacion { get; set; }
    /// <summary>
    /// Columna fecha_utc.
    /// </summary>
    public DateTime? FechaUtc { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_sesion_usuario.
    /// </summary>
    public Guid? IdSesionUsuario { get; set; }
    /// <summary>
    /// Columna codigo_permiso.
    /// </summary>
    public string? CodigoPermiso { get; set; }
    /// <summary>
    /// Columna codigo_operacion.
    /// </summary>
    public string? CodigoOperacion { get; set; }
    /// <summary>
    /// Columna metodo_http.
    /// </summary>
    public string? MetodoHttp { get; set; }
    /// <summary>
    /// Columna permitido.
    /// </summary>
    public bool? Permitido { get; set; }
    /// <summary>
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna id_objeto.
    /// </summary>
    public long? IdObjeto { get; set; }
    /// <summary>
    /// Columna ip_origen.
    /// </summary>
    public string? IpOrigen { get; set; }
    /// <summary>
    /// Columna agente_usuario.
    /// </summary>
    public string? AgenteUsuario { get; set; }
    /// <summary>
    /// Columna solicitud_id.
    /// </summary>
    public string? SolicitudId { get; set; }
}
