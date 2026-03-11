using Secure.Platform.Contracts.Dtos.Organizacion;

namespace Secure.Platform.Data.Repositories.Interfaces.Organizacion;

/// <summary>
/// Contrato del repositorio para organizacion.unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
public interface IUnidadOrganizativaRepository
{
    Task<IReadOnlyList<UnidadOrganizativaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UnidadOrganizativaDto?> ObtenerAsync(long idUnidadOrganizativa, CancellationToken cancellationToken);
    Task<long> CrearAsync(UnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUnidadOrganizativa, string? usuario, CancellationToken cancellationToken);
}
