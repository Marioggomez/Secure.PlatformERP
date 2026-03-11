using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.flujo_restablecimiento_clave.
/// Autor: Mario Gomez.
/// </summary>
public interface IFlujoRestablecimientoClaveRepository
{
    Task<IReadOnlyList<FlujoRestablecimientoClaveDto>> ListarAsync(CancellationToken cancellationToken);
    Task<FlujoRestablecimientoClaveDto?> ObtenerAsync(Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken);
    Task<Guid> CrearAsync(FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(Guid idFlujoRestablecimientoClave, string? usuario, CancellationToken cancellationToken);
}
