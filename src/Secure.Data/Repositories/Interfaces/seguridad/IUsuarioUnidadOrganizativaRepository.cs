using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.usuario_unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
public interface IUsuarioUnidadOrganizativaRepository
{
    Task<IReadOnlyList<UsuarioUnidadOrganizativaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<UsuarioUnidadOrganizativaDto?> ObtenerAsync(long idUsuarioUnidadOrganizativa, CancellationToken cancellationToken);
    Task<long> CrearAsync(UsuarioUnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(UsuarioUnidadOrganizativaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idUsuarioUnidadOrganizativa, string? usuario, CancellationToken cancellationToken);
}
