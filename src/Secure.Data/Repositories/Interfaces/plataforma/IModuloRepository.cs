using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.modulo.
/// Autor: Mario Gomez.
/// </summary>
public interface IModuloRepository
{
    Task<IReadOnlyList<ModuloDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ModuloDto?> ObtenerAsync(int idModulo, CancellationToken cancellationToken);
    Task<int> CrearAsync(ModuloDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ModuloDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idModulo, string? usuario, CancellationToken cancellationToken);
}
