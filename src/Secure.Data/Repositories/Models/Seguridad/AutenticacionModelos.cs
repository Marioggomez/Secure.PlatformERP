namespace Secure.Platform.Data.Repositories.Models.Seguridad;

/// <summary>
/// Contexto de usuario para procesos de autenticacion IAM.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioAutenticacionData
{
    public long IdUsuario { get; set; }
    public long IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
    public string TenantCodigo { get; set; } = string.Empty;
    public string LoginPrincipal { get; set; } = string.Empty;
    public string NombreMostrar { get; set; } = string.Empty;
    public string? CorreoElectronico { get; set; }
    public bool MfaHabilitado { get; set; }
    public bool RequiereCambioClave { get; set; }
    public short IdEstadoUsuario { get; set; }
    public bool ActivoUsuario { get; set; }
    public byte[] HashClave { get; set; } = Array.Empty<byte>();
    public byte[] SaltClave { get; set; } = Array.Empty<byte>();
    public string AlgoritmoClave { get; set; } = string.Empty;
    public int IteracionesClave { get; set; }
    public bool ActivoCredencial { get; set; }
}

/// <summary>
/// Contexto de desafio MFA.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DesafioMfaData
{
    public Guid IdDesafioMfa { get; set; }
    public Guid? IdFlujoAutenticacion { get; set; }
    public long IdUsuario { get; set; }
    public long IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
    public byte[] OtpHash { get; set; } = Array.Empty<byte>();
    public byte[] OtpSalt { get; set; } = Array.Empty<byte>();
    public DateTime ExpiraEnUtc { get; set; }
    public bool Usado { get; set; }
    public short Intentos { get; set; }
    public short MaxIntentos { get; set; }
}

/// <summary>
/// Contexto de token de restablecimiento.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TokenRestablecimientoData
{
    public Guid IdTokenRestablecimientoClave { get; set; }
    public Guid? IdFlujoRestablecimientoClave { get; set; }
    public long IdUsuario { get; set; }
    public DateTime ExpiraEnUtc { get; set; }
    public bool Usado { get; set; }
    public bool FlujoUsado { get; set; }
}

/// <summary>
/// Recurso UI autorizado para un usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiAccesoData
{
    public long IdRecursoUi { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; }
    public string? Componente { get; set; }
    public string? Icono { get; set; }
    public int OrdenVisual { get; set; }
    public long? IdRecursoUiPadre { get; set; }
}
