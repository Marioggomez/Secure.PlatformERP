using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.version_sistema.
/// Autor: Mario Gomez.
/// </summary>
public interface IVersionSistemaRepository
{
    Task<IReadOnlyList<VersionSistemaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<VersionSistemaDto?> ObtenerAsync(int idVersionSistema, CancellationToken cancellationToken);
    Task<int> CrearAsync(VersionSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(VersionSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idVersionSistema, string? usuario, CancellationToken cancellationToken);
}
