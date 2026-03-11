using Secure.Platform.Contracts.Dtos.Organizacion;

namespace Secure.Platform.Data.Repositories.Interfaces.Organizacion;

/// <summary>
/// Contrato del repositorio para organizacion.grupo_empresarial.
/// Autor: Mario Gomez.
/// </summary>
public interface IGrupoEmpresarialRepository
{
    Task<IReadOnlyList<GrupoEmpresarialDto>> ListarAsync(CancellationToken cancellationToken);
    Task<GrupoEmpresarialDto?> ObtenerAsync(long idGrupoEmpresarial, CancellationToken cancellationToken);
    Task<long> CrearAsync(GrupoEmpresarialDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(GrupoEmpresarialDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idGrupoEmpresarial, string? usuario, CancellationToken cancellationToken);
}
