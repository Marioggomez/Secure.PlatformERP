using Secure.Platform.Contracts.Dtos.Organizacion;

namespace Secure.Platform.Data.Repositories.Interfaces.Organizacion;

/// <summary>
/// Contrato del repositorio para organizacion.grupo_empresarial_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IGrupoEmpresarialEmpresaRepository
{
    Task<IReadOnlyList<GrupoEmpresarialEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<GrupoEmpresarialEmpresaDto?> ObtenerAsync(long idGrupoEmpresarial, CancellationToken cancellationToken);
    Task<long> CrearAsync(GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idGrupoEmpresarial, string? usuario, CancellationToken cancellationToken);
}
