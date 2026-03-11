using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.estado_registro.
/// Autor: Mario Gomez.
/// </summary>
public interface IEstadoRegistroRepository
{
    Task<IReadOnlyList<EstadoRegistroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EstadoRegistroDto?> ObtenerAsync(int idEstado, CancellationToken cancellationToken);
    Task<int> CrearAsync(EstadoRegistroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EstadoRegistroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idEstado, string? usuario, CancellationToken cancellationToken);
}
