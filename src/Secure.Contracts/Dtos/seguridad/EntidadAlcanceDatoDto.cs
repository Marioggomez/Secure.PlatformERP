namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.entidad_alcance_dato.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EntidadAlcanceDatoDto
{
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
    /// <summary>
    /// Columna nombre_tabla.
    /// </summary>
    public string? NombreTabla { get; set; }
    /// <summary>
    /// Columna columna_llave_primaria.
    /// </summary>
    public string? ColumnaLlavePrimaria { get; set; }
    /// <summary>
    /// Columna columna_tenant.
    /// </summary>
    public string? ColumnaTenant { get; set; }
    /// <summary>
    /// Columna columna_empresa.
    /// </summary>
    public string? ColumnaEmpresa { get; set; }
    /// <summary>
    /// Columna columna_unidad_organizativa.
    /// </summary>
    public string? ColumnaUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna columna_propietario.
    /// </summary>
    public string? ColumnaPropietario { get; set; }
    /// <summary>
    /// Columna columna_contexto.
    /// </summary>
    public string? ColumnaContexto { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
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
