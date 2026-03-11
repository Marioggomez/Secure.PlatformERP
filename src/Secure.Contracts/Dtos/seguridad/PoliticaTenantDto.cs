namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.politica_tenant.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaTenantDto
{
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna timeout_inactividad_min.
    /// </summary>
    public int? TimeoutInactividadMin { get; set; }
    /// <summary>
    /// Columna timeout_absoluto_min.
    /// </summary>
    public int? TimeoutAbsolutoMin { get; set; }
    /// <summary>
    /// Columna longitud_minima_clave.
    /// </summary>
    public byte? LongitudMinimaClave { get; set; }
    /// <summary>
    /// Columna requiere_mayuscula.
    /// </summary>
    public bool? RequiereMayuscula { get; set; }
    /// <summary>
    /// Columna requiere_minuscula.
    /// </summary>
    public bool? RequiereMinuscula { get; set; }
    /// <summary>
    /// Columna requiere_numero.
    /// </summary>
    public bool? RequiereNumero { get; set; }
    /// <summary>
    /// Columna requiere_especial.
    /// </summary>
    public bool? RequiereEspecial { get; set; }
    /// <summary>
    /// Columna historial_claves.
    /// </summary>
    public byte? HistorialClaves { get; set; }
    /// <summary>
    /// Columna max_intentos_login.
    /// </summary>
    public byte? MaxIntentosLogin { get; set; }
    /// <summary>
    /// Columna minutos_bloqueo.
    /// </summary>
    public int? MinutosBloqueo { get; set; }
    /// <summary>
    /// Columna mfa_obligatorio.
    /// </summary>
    public bool? MfaObligatorio { get; set; }
    /// <summary>
    /// Columna permite_login_local.
    /// </summary>
    public bool? PermiteLoginLocal { get; set; }
    /// <summary>
    /// Columna permite_sso.
    /// </summary>
    public bool? PermiteSso { get; set; }
    /// <summary>
    /// Columna requiere_mfa_aprobaciones.
    /// </summary>
    public bool? RequiereMfaAprobaciones { get; set; }
    /// <summary>
    /// Columna requiere_politica_ip.
    /// </summary>
    public bool? RequierePoliticaIp { get; set; }
    /// <summary>
    /// Columna limite_rate_por_minuto.
    /// </summary>
    public int? LimiteRatePorMinuto { get; set; }
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
