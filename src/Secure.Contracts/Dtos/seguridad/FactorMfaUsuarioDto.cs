namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.factor_mfa_usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FactorMfaUsuarioDto
{
    /// <summary>
    /// Columna id_factor_mfa_usuario.
    /// </summary>
    public long? IdFactorMfaUsuario { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tipo_factor_mfa.
    /// </summary>
    public short? IdTipoFactorMfa { get; set; }
    /// <summary>
    /// Columna etiqueta.
    /// </summary>
    public string? Etiqueta { get; set; }
    /// <summary>
    /// Columna destino_enmascarado.
    /// </summary>
    public string? DestinoEnmascarado { get; set; }
    /// <summary>
    /// Columna referencia_secreto.
    /// </summary>
    public string? ReferenciaSecreto { get; set; }
    /// <summary>
    /// Columna secreto_cifrado.
    /// </summary>
    public byte[]? SecretoCifrado { get; set; }
    /// <summary>
    /// Columna configuracion_json.
    /// </summary>
    public string? ConfiguracionJson { get; set; }
    /// <summary>
    /// Columna verificado.
    /// </summary>
    public bool? Verificado { get; set; }
    /// <summary>
    /// Columna es_predeterminado.
    /// </summary>
    public bool? EsPredeterminado { get; set; }
    /// <summary>
    /// Columna ultimo_uso_utc.
    /// </summary>
    public DateTime? UltimoUsoUtc { get; set; }
    /// <summary>
    /// Columna fecha_enrolamiento_utc.
    /// </summary>
    public DateTime? FechaEnrolamientoUtc { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
