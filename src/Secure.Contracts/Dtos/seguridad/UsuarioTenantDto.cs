namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario_tenant.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioTenantDto
{
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna es_administrador_tenant.
    /// </summary>
    public bool? EsAdministradorTenant { get; set; }
    /// <summary>
    /// Columna es_cuenta_servicio.
    /// </summary>
    public bool? EsCuentaServicio { get; set; }
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
