namespace Secure.Platform.Contracts.Dtos.Common;

/// <summary>
/// Solicitud de paginacion server-side para listados ERP.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PaginacionRequestDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 25;
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public string? Filter { get; set; }
    public string? FilterField { get; set; }
    public long? IdTenant { get; set; }
    public long? IdEmpresa { get; set; }
}

/// <summary>
/// Resultado paginado para consumo de API/WinForms.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PaginacionResultadoDto<TItem>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public string? Filter { get; set; }
    public string? FilterField { get; set; }
    public IReadOnlyList<TItem> Items { get; set; } = Array.Empty<TItem>();
}
