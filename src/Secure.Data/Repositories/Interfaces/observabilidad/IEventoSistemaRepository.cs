using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.evento_sistema.
/// Autor: Mario Gomez.
/// </summary>
public interface IEventoSistemaRepository
{
    Task<IReadOnlyList<EventoSistemaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EventoSistemaDto?> ObtenerAsync(long idEventoSistema, CancellationToken cancellationToken);
    Task<long> CrearAsync(EventoSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EventoSistemaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idEventoSistema, string? usuario, CancellationToken cancellationToken);
}
