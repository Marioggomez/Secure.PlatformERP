using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.control_intentos_login.
/// Autor: Mario Gomez.
/// </summary>
public interface IControlIntentosLoginRepository
{
    Task<IReadOnlyList<ControlIntentosLoginDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ControlIntentosLoginDto?> ObtenerAsync(long idControlIntento, CancellationToken cancellationToken);
    Task<long> CrearAsync(ControlIntentosLoginDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ControlIntentosLoginDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idControlIntento, string? usuario, CancellationToken cancellationToken);
}
