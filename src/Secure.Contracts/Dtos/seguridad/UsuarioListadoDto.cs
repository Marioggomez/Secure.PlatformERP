namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// DTO ligero para listados paginados de usuarios.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioListadoDto
{
    public long IdUsuario { get; set; }
    public long? IdTenant { get; set; }
    public string LoginPrincipal { get; set; } = string.Empty;
    public string NombreMostrar { get; set; } = string.Empty;
    public string? CorreoElectronico { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? UltimoAccesoUtc { get; set; }
    public bool Activo { get; set; }
}
