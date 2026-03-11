namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.dispositivo_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DispositivoUsuarioDto
{
    /// <summary>
    /// Columna id_dispositivo_usuario.
    /// </summary>
    public long? IdDispositivoUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna fingerprint.
    /// </summary>
    public string? Fingerprint { get; set; }
    /// <summary>
    /// Columna navegador.
    /// </summary>
    public string? Navegador { get; set; }
    /// <summary>
    /// Columna sistema_operativo.
    /// </summary>
    public string? SistemaOperativo { get; set; }
    /// <summary>
    /// Columna ip_ultimo_acceso.
    /// </summary>
    public string? IpUltimoAcceso { get; set; }
    /// <summary>
    /// Columna fecha_registro.
    /// </summary>
    public DateTime? FechaRegistro { get; set; }
    /// <summary>
    /// Columna fecha_ultimo_acceso.
    /// </summary>
    public DateTime? FechaUltimoAcceso { get; set; }
}
