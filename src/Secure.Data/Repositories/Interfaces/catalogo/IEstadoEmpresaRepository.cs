using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.estado_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IEstadoEmpresaRepository
{
    Task<IReadOnlyList<EstadoEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EstadoEmpresaDto?> ObtenerAsync(short idEstadoEmpresa, CancellationToken cancellationToken);
    Task<short> CrearAsync(EstadoEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EstadoEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idEstadoEmpresa, string? usuario, CancellationToken cancellationToken);
}
