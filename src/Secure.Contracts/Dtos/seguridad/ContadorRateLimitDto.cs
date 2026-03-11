namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.contador_rate_limit.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ContadorRateLimitDto
{
    /// <summary>
    /// Columna id_contador_rate_limit.
    /// </summary>
    public long? IdContadorRateLimit { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna ambito.
    /// </summary>
    public string? Ambito { get; set; }
    /// <summary>
    /// Columna llave.
    /// </summary>
    public string? Llave { get; set; }
    /// <summary>
    /// Columna endpoint.
    /// </summary>
    public string? Endpoint { get; set; }
    /// <summary>
    /// Columna inicio_ventana_utc.
    /// </summary>
    public DateTime? InicioVentanaUtc { get; set; }
    /// <summary>
    /// Columna conteo.
    /// </summary>
    public int? Conteo { get; set; }
}
