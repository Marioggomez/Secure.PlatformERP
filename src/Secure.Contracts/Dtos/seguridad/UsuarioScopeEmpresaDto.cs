namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO de la tabla seguridad.usuario_scope_empresa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioScopeEmpresaDto
{
    /// <summary>
    /// Columna id_usuario_scope_empresa.
    /// </summary>
    public long? IdUsuarioScopeEmpresa { get; set; }
    /// <summary>
    /// Columna id_usuario.
    /// </summary>
    public long? IdUsuario { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
}
