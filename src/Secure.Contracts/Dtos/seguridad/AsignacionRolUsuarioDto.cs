namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.asignacion_rol_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AsignacionRolUsuarioDto
{
    /// <summary>
    /// Columna id_asignacion_rol_usuario.
    /// </summary>
    public long? IdAsignacionRolUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_rol.
    /// </summary>
    public long? IdRol { get; set; }
    /// <summary>
    /// Columna id_alcance_asignacion.
    /// </summary>
    public short? IdAlcanceAsignacion { get; set; }
    /// <summary>
    /// Columna id_grupo_empresarial.
    /// </summary>
    public long? IdGrupoEmpresarial { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_unidad_organizativa.
    /// </summary>
    public long? IdUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna fecha_inicio_utc.
    /// </summary>
    public DateTime? FechaInicioUtc { get; set; }
    /// <summary>
    /// Columna fecha_fin_utc.
    /// </summary>
    public DateTime? FechaFinUtc { get; set; }
    /// <summary>
    /// Columna concedido_por.
    /// </summary>
    public long? ConcedidoPor { get; set; }
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
