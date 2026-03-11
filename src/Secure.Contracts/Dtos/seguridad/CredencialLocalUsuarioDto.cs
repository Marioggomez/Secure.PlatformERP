namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.credencial_local_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CredencialLocalUsuarioDto
{
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
    /// Columna cambio_clave_utc.
    /// </summary>
    public DateTime? CambioClaveUtc { get; set; }
    /// <summary>
    /// Columna debe_cambiar_clave.
    /// </summary>
    public bool? DebeCambiarClave { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
