namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.paso_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PasoInstanciaAprobacionDto
{
    /// <summary>
    /// Columna id_paso_instancia_aprobacion.
    /// </summary>
    public long? IdPasoInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna id_instancia_aprobacion.
    /// </summary>
    public long? IdInstanciaAprobacion { get; set; }
    /// <summary>
    /// Columna nivel_orden.
    /// </summary>
    public byte? NivelOrden { get; set; }
    /// <summary>
    /// Columna id_estado_aprobacion.
    /// </summary>
    public short? IdEstadoAprobacion { get; set; }
    /// <summary>
    /// Columna iniciado_utc.
    /// </summary>
    public DateTime? IniciadoUtc { get; set; }
    /// <summary>
    /// Columna resuelto_utc.
    /// </summary>
    public DateTime? ResueltoUtc { get; set; }
}
