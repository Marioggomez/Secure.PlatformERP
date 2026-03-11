using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.error_aplicacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IErrorAplicacionRepository
{
    Task<IReadOnlyList<ErrorAplicacionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ErrorAplicacionDto?> ObtenerAsync(long idErrorAplicacion, CancellationToken cancellationToken);
    Task<long> CrearAsync(ErrorAplicacionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ErrorAplicacionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idErrorAplicacion, string? usuario, CancellationToken cancellationToken);
}
