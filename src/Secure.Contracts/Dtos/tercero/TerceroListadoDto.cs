namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO ligero para listados paginados de terceros.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TerceroListadoDto
{
    public long IdTercero { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int IdTipoPersona { get; set; }
    public string TipoPersona { get; set; } = string.Empty;
    public string NombrePrincipal { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime CreadoUtc { get; set; }
}
