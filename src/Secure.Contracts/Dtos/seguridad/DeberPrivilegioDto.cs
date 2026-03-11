namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.deber_privilegio.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DeberPrivilegioDto
{
    /// <summary>
    /// Columna id_deber.
    /// </summary>
    public long? IdDeber { get; set; }
    /// <summary>
    /// Columna id_privilegio.
    /// </summary>
    public long? IdPrivilegio { get; set; }
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
