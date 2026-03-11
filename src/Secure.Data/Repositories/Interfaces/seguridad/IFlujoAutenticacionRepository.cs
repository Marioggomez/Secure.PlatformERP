using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.flujo_autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IFlujoAutenticacionRepository
{
    Task<IReadOnlyList<FlujoAutenticacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<FlujoAutenticacionDto?> ObtenerAsync(Guid idFlujoAutenticacion, CancellationToken cancellationToken);
    Task<Guid> CrearAsync(FlujoAutenticacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(FlujoAutenticacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(Guid idFlujoAutenticacion, string? usuario, CancellationToken cancellationToken);
}
