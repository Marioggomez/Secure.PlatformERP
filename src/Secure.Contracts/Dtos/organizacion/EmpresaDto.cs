namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO de la tabla organizacion.empresa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EmpresaDto
{
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna nombre_legal.
    /// </summary>
    public string? NombreLegal { get; set; }
    /// <summary>
    /// Columna id_tipo_empresa.
    /// </summary>
    public short? IdTipoEmpresa { get; set; }
    /// <summary>
    /// Columna id_estado_empresa.
    /// </summary>
    public short? IdEstadoEmpresa { get; set; }
    /// <summary>
    /// Columna identificacion_fiscal.
    /// </summary>
    public string? IdentificacionFiscal { get; set; }
    /// <summary>
    /// Columna moneda_base.
    /// </summary>
    public string? MonedaBase { get; set; }
    /// <summary>
    /// Columna zona_horaria.
    /// </summary>
    public string? ZonaHoraria { get; set; }
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
