namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.filtro_dato_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FiltroDatoUsuarioDto
{
    /// <summary>
    /// Columna id_filtro_dato_usuario.
    /// </summary>
    public long? IdFiltroDatoUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna id_modo_filtro_dato.
    /// </summary>
    public short? IdModoFiltroDato { get; set; }
    /// <summary>
    /// Columna valor_filtro.
    /// </summary>
    public string? ValorFiltro { get; set; }
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
