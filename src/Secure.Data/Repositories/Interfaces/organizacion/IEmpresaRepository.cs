using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Organizacion;

namespace Secure.Platform.Data.Repositories.Interfaces.Organizacion;

/// <summary>
/// Contrato del repositorio para organizacion.empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IEmpresaRepository
{
    Task<IReadOnlyList<EmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PaginacionResultadoDto<EmpresaListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken);
    Task<EmpresaDto?> ObtenerAsync(long idEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(EmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(EmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idEmpresa, string? usuario, CancellationToken cancellationToken);
}
