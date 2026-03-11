namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.auditoria_operacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaOperacionDto
{
    /// <summary>
    /// Columna id_auditoria.
    /// </summary>
    public long? IdAuditoria { get; set; }
    /// <summary>
    /// Columna tabla.
    /// </summary>
    public string? Tabla { get; set; }
    /// <summary>
    /// Columna operacion.
    /// </summary>
    public string? Operacion { get; set; }
    /// <summary>
    /// Columna id_registro.
    /// </summary>
    public long? IdRegistro { get; set; }
    /// <summary>
    /// Columna valores_anteriores.
    /// </summary>
    public string? ValoresAnteriores { get; set; }
    /// <summary>
    /// Columna valores_nuevos.
    /// </summary>
    public string? ValoresNuevos { get; set; }
    /// <summary>
    /// Columna usuario.
    /// </summary>
    public string? Usuario { get; set; }
    /// <summary>
    /// Columna correlation_id.
    /// </summary>
    public Guid? CorrelationId { get; set; }
    /// <summary>
    /// Columna fecha.
    /// </summary>
    public DateTime? Fecha { get; set; }
}
