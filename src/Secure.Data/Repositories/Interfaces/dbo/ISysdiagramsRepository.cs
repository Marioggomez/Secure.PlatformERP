using Secure.Platform.Contracts.Dtos.Dbo;

namespace Secure.Platform.Data.Repositories.Interfaces.Dbo;

/// <summary>
/// Contrato del repositorio para dbo.sysdiagrams.
/// Autor: Mario Gomez.
/// </summary>
public interface ISysdiagramsRepository
{
    Task<IReadOnlyList<SysdiagramsDto>> ListarAsync(CancellationToken cancellationToken);
    Task<SysdiagramsDto?> ObtenerAsync(string name, CancellationToken cancellationToken);
    Task<string> CrearAsync(SysdiagramsDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(SysdiagramsDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(string name, string? usuario, CancellationToken cancellationToken);
}
