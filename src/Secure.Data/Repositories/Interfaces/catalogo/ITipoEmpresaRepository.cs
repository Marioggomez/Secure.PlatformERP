using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoEmpresaRepository
{
    Task<IReadOnlyList<TipoEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoEmpresaDto?> ObtenerAsync(short idTipoEmpresa, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoEmpresa, string? usuario, CancellationToken cancellationToken);
}
