namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.historial_clave_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class HistorialClaveUsuarioDto
{
    /// <summary>
    /// Columna id_historial_clave_usuario.
    /// </summary>
    public long? IdHistorialClaveUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna hash_clave.
    /// </summary>
    public byte[]? HashClave { get; set; }
    /// <summary>
    /// Columna salt_clave.
    /// </summary>
    public byte[]? SaltClave { get; set; }
    /// <summary>
    /// Columna algoritmo_clave.
    /// </summary>
    public string? AlgoritmoClave { get; set; }
    /// <summary>
    /// Columna iteraciones_clave.
    /// </summary>
    public int? IteracionesClave { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
