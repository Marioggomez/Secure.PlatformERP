namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.operacion_api.
/// Autor: Mario Gomez.
/// </summary>
public sealed class OperacionApiDto
{
    /// <summary>
    /// Columna id_operacion_api.
    /// </summary>
    public long? IdOperacionApi { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna modulo.
    /// </summary>
    public string? Modulo { get; set; }
    /// <summary>
    /// Columna controlador.
    /// </summary>
    public string? Controlador { get; set; }
    /// <summary>
    /// Columna accion.
    /// </summary>
    public string? Accion { get; set; }
    /// <summary>
    /// Columna metodo_http.
    /// </summary>
    public string? MetodoHttp { get; set; }
    /// <summary>
    /// Columna ruta.
    /// </summary>
    public string? Ruta { get; set; }
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
