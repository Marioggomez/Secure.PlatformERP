namespace Secure.Platform.Contracts.Dtos.Observabilidad;

/// <summary>
/// DTO de la tabla observabilidad.evento_sistema.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EventoSistemaDto
{
    /// <summary>
    /// Columna id_evento_sistema.
    /// </summary>
    public long? IdEventoSistema { get; set; }
    /// <summary>
    /// Columna correlation_id.
    /// </summary>
    public Guid? CorrelationId { get; set; }
    /// <summary>
    /// Columna tipo_evento.
    /// </summary>
    public string? TipoEvento { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna usuario.
    /// </summary>
    public string? Usuario { get; set; }
    /// <summary>
    /// Columna fecha.
    /// </summary>
    public DateTime? Fecha { get; set; }
}
