using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.version_esquema.
/// Autor: Mario Gomez.
/// </summary>
public interface IVersionEsquemaRepository
{
    Task<IReadOnlyList<VersionEsquemaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<VersionEsquemaDto?> ObtenerAsync(long idVersionEsquema, CancellationToken cancellationToken);
    Task<long> CrearAsync(VersionEsquemaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(VersionEsquemaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idVersionEsquema, string? usuario, CancellationToken cancellationToken);
}
