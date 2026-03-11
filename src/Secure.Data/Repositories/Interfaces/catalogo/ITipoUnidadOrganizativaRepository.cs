using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoUnidadOrganizativaRepository
{
    Task<IReadOnlyList<TipoUnidadOrganizativaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoUnidadOrganizativaDto?> ObtenerAsync(short idTipoUnidadOrganizativa, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoUnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoUnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoUnidadOrganizativa, string? usuario, CancellationToken cancellationToken);
}
