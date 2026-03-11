namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO ligero para listados paginados de empresas.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EmpresaListadoDto
{
    public long IdEmpresa { get; set; }
    public long IdTenant { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? NombreLegal { get; set; }
    public string? IdentificacionFiscal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
