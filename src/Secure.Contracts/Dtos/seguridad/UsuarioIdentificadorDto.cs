namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario_identificador.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioIdentificadorDto
{
    /// <summary>
    /// Columna id_usuario_identificador.
    /// </summary>
    public long? IdUsuarioIdentificador { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_tipo_identificador_usuario.
    /// </summary>
    public short? IdTipoIdentificadorUsuario { get; set; }
    /// <summary>
    /// Columna valor.
    /// </summary>
    public string? Valor { get; set; }
    /// <summary>
    /// Columna valor_normalizado.
    /// </summary>
    public string? ValorNormalizado { get; set; }
    /// <summary>
    /// Columna es_principal.
    /// </summary>
    public bool? EsPrincipal { get; set; }
    /// <summary>
    /// Columna verificado.
    /// </summary>
    public bool? Verificado { get; set; }
    /// <summary>
    /// Columna fecha_verificacion_utc.
    /// </summary>
    public DateTime? FechaVerificacionUtc { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
