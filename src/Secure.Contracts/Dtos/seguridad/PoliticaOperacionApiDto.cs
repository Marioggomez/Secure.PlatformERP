namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.politica_operacion_api.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaOperacionApiDto
{
    /// <summary>
    /// Columna id_politica_operacion_api.
    /// </summary>
    public long? IdPoliticaOperacionApi { get; set; }
    /// <summary>
    /// Columna id_operacion_api.
    /// </summary>
    public long? IdOperacionApi { get; set; }
    /// <summary>
    /// Columna id_permiso.
    /// </summary>
    public int? IdPermiso { get; set; }
    /// <summary>
    /// Columna requiere_autenticacion.
    /// </summary>
    public bool? RequiereAutenticacion { get; set; }
    /// <summary>
    /// Columna requiere_sesion.
    /// </summary>
    public bool? RequiereSesion { get; set; }
    /// <summary>
    /// Columna requiere_empresa.
    /// </summary>
    public bool? RequiereEmpresa { get; set; }
    /// <summary>
    /// Columna requiere_unidad_organizativa.
    /// </summary>
    public bool? RequiereUnidadOrganizativa { get; set; }
    /// <summary>
    /// Columna requiere_mfa.
    /// </summary>
    public bool? RequiereMfa { get; set; }
    /// <summary>
    /// Columna requiere_auditoria.
    /// </summary>
    public bool? RequiereAuditoria { get; set; }
    /// <summary>
    /// Columna requiere_aprobacion.
    /// </summary>
    public bool? RequiereAprobacion { get; set; }
    /// <summary>
    /// Columna codigo_entidad.
    /// </summary>
    public string? CodigoEntidad { get; set; }
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
