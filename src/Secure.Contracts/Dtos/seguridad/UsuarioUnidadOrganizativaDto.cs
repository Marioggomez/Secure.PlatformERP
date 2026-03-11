namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario_unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioUnidadOrganizativaDto
{
    /// <summary>
    /// Columna id_usuario_unidad_organizativa.
    /// </summary>
    public long? IdUsuarioUnidadOrganizativa { get; set; }
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
    /// Columna rol_operativo.
    /// </summary>
    public string? RolOperativo { get; set; }
    /// <summary>
    /// Columna fecha_inicio_utc.
    /// </summary>
    public DateTime? FechaInicioUtc { get; set; }
    /// <summary>
    /// Columna fecha_fin_utc.
    /// </summary>
    public DateTime? FechaFinUtc { get; set; }
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
