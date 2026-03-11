namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.excepcion_permiso_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ExcepcionPermisoUsuarioDto
{
    /// <summary>
    /// Columna id_excepcion_permiso_usuario.
    /// </summary>
    public long? IdExcepcionPermisoUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_permiso.
    /// </summary>
    public int? IdPermiso { get; set; }
    /// <summary>
    /// Columna id_efecto_permiso.
    /// </summary>
    public short? IdEfectoPermiso { get; set; }
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
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
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
