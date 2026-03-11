using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.entidad_alcance_dato.
/// Autor: Mario Gomez.
/// </summary>
public interface IEntidadAlcanceDatoRepository
{
    Task<IReadOnlyList<EntidadAlcanceDatoDto>> ListarAsync(CancellationToken cancellationToken);
    Task<EntidadAlcanceDatoDto?> ObtenerAsync(string codigoEntidad, CancellationToken cancellationToken);
    Task<string> CrearAsync(EntidadAlcanceDatoDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EntidadAlcanceDatoDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(string codigoEntidad, string? usuario, CancellationToken cancellationToken);
}
