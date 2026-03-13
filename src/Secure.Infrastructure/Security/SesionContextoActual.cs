namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Contexto de sesion resuelto desde token opaco.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionContextoActual
{
    public Guid IdSesionUsuario { get; set; }
    public long IdUsuario { get; set; }
    public long IdTenant { get; set; }
    public long IdEmpresa { get; set; }
    public bool MfaValidado { get; set; }
    public bool Activo { get; set; }
    public DateTime ExpiraAbsolutaUtc { get; set; }
    public string? UsuarioMostrar { get; set; }
}
