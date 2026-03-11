namespace Secure.Platform.Contracts.Dtos.Seguridad;

/// <summary>
/// Solicitud para guardar layout de interfaz por usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class GuardarLayoutUiRequestDto
{
    public long IdUsuario { get; set; }
    public long IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
    public string CodigoLayout { get; set; } = string.Empty;
    public string LayoutPayload { get; set; } = string.Empty;
}

/// <summary>
/// Respuesta de guardado de layout.
/// Autor: Mario Gomez.
/// </summary>
public sealed class GuardarLayoutUiResponseDto
{
    public bool Guardado { get; set; }
    public string CodigoEntidad { get; set; } = string.Empty;
    public DateTime ActualizadoUtc { get; set; }
}

/// <summary>
/// Respuesta de lectura de layout.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ObtenerLayoutUiResponseDto
{
    public bool Encontrado { get; set; }
    public string CodigoEntidad { get; set; } = string.Empty;
    public string? LayoutPayload { get; set; }
    public DateTime? ActualizadoUtc { get; set; }
}
