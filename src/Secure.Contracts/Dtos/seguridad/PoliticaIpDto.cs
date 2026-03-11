namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.politica_ip.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaIpDto
{
    /// <summary>
    /// Columna id_politica_ip.
    /// </summary>
    public long? IdPoliticaIp { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna ip_o_cidr.
    /// </summary>
    public string? IpOCidr { get; set; }
    /// <summary>
    /// Columna accion.
    /// </summary>
    public string? Accion { get; set; }
    /// <summary>
    /// Columna prioridad.
    /// </summary>
    public int? Prioridad { get; set; }
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
