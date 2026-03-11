namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario_scope_unidad.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioScopeUnidadDto
{
    /// <summary>
    /// Columna id_usuario_scope_unidad.
    /// </summary>
    public long? IdUsuarioScopeUnidad { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_unidad_organizativa.
    /// </summary>
    public long? IdUnidadOrganizativa { get; set; }
}
