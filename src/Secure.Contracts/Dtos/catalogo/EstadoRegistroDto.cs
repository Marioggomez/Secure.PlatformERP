namespace Secure.Platform.Contracts.Dtos.Catalogo;

/// <summary>
/// DTO de la tabla catalogo.estado_registro.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EstadoRegistroDto
{
    /// <summary>
    /// Columna id_estado.
    /// </summary>
    public int? IdEstado { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
