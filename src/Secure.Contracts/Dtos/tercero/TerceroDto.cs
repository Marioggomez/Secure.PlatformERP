namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.tercero.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TerceroDto
{
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna id_tipo_persona.
    /// </summary>
    public int? IdTipoPersona { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna segundo_nombre.
    /// </summary>
    public string? SegundoNombre { get; set; }
    /// <summary>
    /// Columna apellido.
    /// </summary>
    public string? Apellido { get; set; }
    /// <summary>
    /// Columna segundo_apellido.
    /// </summary>
    public string? SegundoApellido { get; set; }
    /// <summary>
    /// Columna razon_social.
    /// </summary>
    public string? RazonSocial { get; set; }
    /// <summary>
    /// Columna nombre_comercial.
    /// </summary>
    public string? NombreComercial { get; set; }
    /// <summary>
    /// Columna fecha_nacimiento.
    /// </summary>
    public DateTime? FechaNacimiento { get; set; }
    /// <summary>
    /// Columna fecha_constitucion.
    /// </summary>
    public DateTime? FechaConstitucion { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_por.
    /// </summary>
    public int? CreadoPor { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
