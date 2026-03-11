using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.deber_privilegio.
/// Autor: Mario Gomez.
/// </summary>
public interface IDeberPrivilegioRepository
{
    Task<IReadOnlyList<DeberPrivilegioDto>> ListarAsync(CancellationToken cancellationToken);
    Task<DeberPrivilegioDto?> ObtenerAsync(long idDeber, CancellationToken cancellationToken);
    Task<long> CrearAsync(DeberPrivilegioDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(DeberPrivilegioDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idDeber, string? usuario, CancellationToken cancellationToken);
}
