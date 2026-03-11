namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.rol_deber.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RolDeberDto
{
    /// <summary>
    /// Columna id_rol.
    /// </summary>
    public long? IdRol { get; set; }
    /// <summary>
    /// Columna id_deber.
    /// </summary>
    public long? IdDeber { get; set; }
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
