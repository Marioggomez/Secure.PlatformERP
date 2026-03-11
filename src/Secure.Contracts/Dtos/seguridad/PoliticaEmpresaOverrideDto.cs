namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.politica_empresa_override.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaEmpresaOverrideDto
{
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna timeout_inactividad_min_override.
    /// </summary>
    public int? TimeoutInactividadMinOverride { get; set; }
    /// <summary>
    /// Columna timeout_absoluto_min_override.
    /// </summary>
    public int? TimeoutAbsolutoMinOverride { get; set; }
    /// <summary>
    /// Columna mfa_obligatorio_override.
    /// </summary>
    public bool? MfaObligatorioOverride { get; set; }
    /// <summary>
    /// Columna max_intentos_login_override.
    /// </summary>
    public byte? MaxIntentosLoginOverride { get; set; }
    /// <summary>
    /// Columna minutos_bloqueo_override.
    /// </summary>
    public int? MinutosBloqueoOverride { get; set; }
    /// <summary>
    /// Columna requiere_politica_ip_override.
    /// </summary>
    public bool? RequierePoliticaIpOverride { get; set; }
    /// <summary>
    /// Columna requiere_mfa_aprobaciones_override.
    /// </summary>
    public bool? RequiereMfaAprobacionesOverride { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
