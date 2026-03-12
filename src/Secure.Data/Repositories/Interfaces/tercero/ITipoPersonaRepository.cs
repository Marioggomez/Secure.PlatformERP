using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.tipo_persona.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoPersonaRepository
{
    Task<IReadOnlyList<TipoPersonaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoPersonaDto?> ObtenerAsync(int idTipoPersona, CancellationToken cancellationToken);
    Task<int> CrearAsync(TipoPersonaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoPersonaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idTipoPersona, string? usuario, CancellationToken cancellationToken);
}
