using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.modo_filtro_dato.
/// Autor: Mario Gomez.
/// </summary>
public interface IModoFiltroDatoRepository
{
    Task<IReadOnlyList<ModoFiltroDatoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ModoFiltroDatoDto?> ObtenerAsync(short idModoFiltroDato, CancellationToken cancellationToken);
    Task<short> CrearAsync(ModoFiltroDatoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ModoFiltroDatoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idModoFiltroDato, string? usuario, CancellationToken cancellationToken);
}
