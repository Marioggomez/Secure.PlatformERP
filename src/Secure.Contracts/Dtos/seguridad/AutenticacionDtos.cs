namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// Solicitud para iniciar autenticacion primaria.
/// Autor: Mario Gomez.
/// </summary>
public sealed class LoginRequestDto
{
    public string TenantCodigo { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string? IpOrigen { get; set; }
    public string? AgenteUsuario { get; set; }
    public string? HuellaDispositivo { get; set; }
    public string? SolicitudId { get; set; }
}

/// <summary>
/// Respuesta del inicio de autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class LoginResponseDto
{
    public bool Autenticado { get; set; }
    public bool RequiereMfa { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public Guid? IdFlujoAutenticacion { get; set; }
    public Guid? IdDesafioMfa { get; set; }
    public string? CodigoMfaPrueba { get; set; }
    public string? TokenSesion { get; set; }
    public DateTime? ExpiraSesionUtc { get; set; }
    public long? IdUsuario { get; set; }
    public long? IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
    public string? UsuarioMostrar { get; set; }
    public IReadOnlyList<string> Permisos { get; set; } = Array.Empty<string>();
    public IReadOnlyList<RecursoUiAccesoDto> RecursosUi { get; set; } = Array.Empty<RecursoUiAccesoDto>();
}

/// <summary>
/// Solicitud de validacion de codigo MFA.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ValidarMfaRequestDto
{
    public Guid IdFlujoAutenticacion { get; set; }
    public Guid IdDesafioMfa { get; set; }
    public string CodigoOtp { get; set; } = string.Empty;
    public string? IpOrigen { get; set; }
    public string? AgenteUsuario { get; set; }
    public string? HuellaDispositivo { get; set; }
    public string? SolicitudId { get; set; }
}

/// <summary>
/// Respuesta de validacion MFA.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ValidarMfaResponseDto
{
    public bool Validado { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? TokenSesion { get; set; }
    public DateTime? ExpiraSesionUtc { get; set; }
    public long? IdUsuario { get; set; }
    public long? IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
    public string? UsuarioMostrar { get; set; }
    public IReadOnlyList<string> Permisos { get; set; } = Array.Empty<string>();
    public IReadOnlyList<RecursoUiAccesoDto> RecursosUi { get; set; } = Array.Empty<RecursoUiAccesoDto>();
}

/// <summary>
/// Solicitud para reenviar desafio MFA.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ReenviarMfaRequestDto
{
    public Guid IdFlujoAutenticacion { get; set; }
    public Guid IdDesafioMfa { get; set; }
}

/// <summary>
/// Respuesta al reenviar desafio MFA.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ReenviarMfaResponseDto
{
    public bool Reenviado { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public Guid? IdDesafioMfa { get; set; }
    public DateTime? ExpiraEnUtc { get; set; }
    public string? CodigoMfaPrueba { get; set; }
}

/// <summary>
/// Solicitud para iniciar flujo de restablecimiento.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IniciarRestablecimientoClaveRequestDto
{
    public string TenantCodigo { get; set; } = string.Empty;
    public string UsuarioOCorreo { get; set; } = string.Empty;
    public short IdTipoVerificacionRestablecimiento { get; set; } = 1;
    public string? IpOrigen { get; set; }
    public string? AgenteUsuario { get; set; }
    public string? SolicitudId { get; set; }
}

/// <summary>
/// Respuesta al iniciar restablecimiento de clave.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IniciarRestablecimientoClaveResponseDto
{
    public bool Iniciado { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public Guid? IdFlujoRestablecimientoClave { get; set; }
    public string? TokenRestablecimientoPrueba { get; set; }
    public DateTime? ExpiraEnUtc { get; set; }
}

/// <summary>
/// Solicitud para completar restablecimiento de clave.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CompletarRestablecimientoClaveRequestDto
{
    public Guid IdFlujoRestablecimientoClave { get; set; }
    public string TokenRestablecimiento { get; set; } = string.Empty;
    public string NuevaContrasena { get; set; } = string.Empty;
    public string? IpOrigen { get; set; }
    public string? AgenteUsuario { get; set; }
}

/// <summary>
/// Respuesta al completar restablecimiento de clave.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CompletarRestablecimientoClaveResponseDto
{
    public bool Restablecido { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}

/// <summary>
/// Recurso de interfaz disponible para el usuario autenticado.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiAccesoDto
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
