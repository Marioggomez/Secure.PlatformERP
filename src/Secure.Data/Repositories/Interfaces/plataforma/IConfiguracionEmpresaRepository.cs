using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.configuracion_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IConfiguracionEmpresaRepository
{
    Task<IReadOnlyList<ConfiguracionEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ConfiguracionEmpresaDto?> ObtenerAsync(long idConfiguracionEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(ConfiguracionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ConfiguracionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idConfiguracionEmpresa, string? usuario, CancellationToken cancellationToken);
}
