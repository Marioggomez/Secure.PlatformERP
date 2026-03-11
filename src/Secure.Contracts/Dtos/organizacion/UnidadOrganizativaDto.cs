namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO de la tabla organizacion.unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UnidadOrganizativaDto
{
    /// <summary>
    /// Columna id_unidad_organizativa.
    /// </summary>
    public long? IdUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_tipo_unidad_organizativa.
    /// </summary>
    public short? IdTipoUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna id_unidad_padre.
    /// </summary>
    public long? IdUnidadPadre { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna nivel_jerarquia.
    /// </summary>
    public short? NivelJerarquia { get; set; }
    /// <summary>
    /// Columna ruta_jerarquia.
    /// </summary>
    public string? RutaJerarquia { get; set; }
    /// <summary>
    /// Columna es_hoja.
    /// </summary>
    public bool? EsHoja { get; set; }
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
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
