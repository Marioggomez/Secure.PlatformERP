using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.politica_empresa_override.
/// Autor: Mario Gomez.
/// </summary>
public interface IPoliticaEmpresaOverrideRepository
{
    Task<IReadOnlyList<PoliticaEmpresaOverrideDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PoliticaEmpresaOverrideDto?> ObtenerAsync(long idEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idEmpresa, string? usuario, CancellationToken cancellationToken);
}
