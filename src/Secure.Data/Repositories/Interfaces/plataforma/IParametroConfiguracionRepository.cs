using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.parametro_configuracion.
/// Autor: Mario Gomez.
/// </summary>
public interface IParametroConfiguracionRepository
{
    Task<IReadOnlyList<ParametroConfiguracionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ParametroConfiguracionDto?> ObtenerAsync(int idParametroConfiguracion, CancellationToken cancellationToken);
    Task<int> CrearAsync(ParametroConfiguracionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ParametroConfiguracionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idParametroConfiguracion, string? usuario, CancellationToken cancellationToken);
}
