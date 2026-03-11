namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.ip_bloqueada.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IpBloqueadaDto
{
    /// <summary>
    /// Columna id_ip_bloqueada.
    /// </summary>
    public long? IdIpBloqueada { get; set; }
    /// <summary>
    /// Columna ip.
    /// </summary>
    public string? Ip { get; set; }
    /// <summary>
    /// Columna motivo.
    /// </summary>
    public string? Motivo { get; set; }
    /// <summary>
    /// Columna fecha_bloqueo.
    /// </summary>
    public DateTime? FechaBloqueo { get; set; }
    /// <summary>
    /// Columna fecha_expiracion.
    /// </summary>
    public DateTime? FechaExpiracion { get; set; }
}
