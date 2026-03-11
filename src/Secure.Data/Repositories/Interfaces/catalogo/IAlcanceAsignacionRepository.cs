using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.alcance_asignacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IAlcanceAsignacionRepository
{
    Task<IReadOnlyList<AlcanceAsignacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AlcanceAsignacionDto?> ObtenerAsync(short idAlcanceAsignacion, CancellationToken cancellationToken);
    Task<short> CrearAsync(AlcanceAsignacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AlcanceAsignacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idAlcanceAsignacion, string? usuario, CancellationToken cancellationToken);
}
