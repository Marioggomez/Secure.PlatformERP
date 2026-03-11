using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_relacion_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoRelacionEmpresaRepository
{
    Task<IReadOnlyList<TipoRelacionEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoRelacionEmpresaDto?> ObtenerAsync(short idTipoRelacionEmpresa, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoRelacionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoRelacionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoRelacionEmpresa, string? usuario, CancellationToken cancellationToken);
}
