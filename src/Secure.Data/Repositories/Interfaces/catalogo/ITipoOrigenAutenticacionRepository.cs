using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_origen_autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoOrigenAutenticacionRepository
{
    Task<IReadOnlyList<TipoOrigenAutenticacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoOrigenAutenticacionDto?> ObtenerAsync(short idTipoOrigenAutenticacion, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoOrigenAutenticacion, string? usuario, CancellationToken cancellationToken);
}
