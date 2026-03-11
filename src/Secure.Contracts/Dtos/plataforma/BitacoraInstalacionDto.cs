namespace Secure.Platform.Contracts.Dtos.Plataforma;

/// <summary>
/// DTO de la tabla plataforma.bitacora_instalacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class BitacoraInstalacionDto
{
    /// <summary>
    /// Columna id_bitacora_instalacion.
    /// </summary>
    public long? IdBitacoraInstalacion { get; set; }
    /// <summary>
    /// Columna componente.
    /// </summary>
    public string? Componente { get; set; }
    /// <summary>
    /// Columna accion.
    /// </summary>
    public string? Accion { get; set; }
    /// <summary>
    /// Columna estado.
    /// </summary>
    public string? Estado { get; set; }
    /// <summary>
    /// Columna detalle.
    /// </summary>
    public string? Detalle { get; set; }
    /// <summary>
    /// Columna iniciado_utc.
    /// </summary>
    public DateTime? IniciadoUtc { get; set; }
    /// <summary>
    /// Columna finalizado_utc.
    /// </summary>
    public DateTime? FinalizadoUtc { get; set; }
}
