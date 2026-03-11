using Secure.Platform.Contracts.Dtos.Organizacion;

namespace Secure.Platform.Data.Repositories.Interfaces.Organizacion;

/// <summary>
/// Contrato del repositorio para organizacion.relacion_empresa.
/// Autor: Mario Gomez.
/// </summary>
public interface IRelacionEmpresaRepository
{
    Task<IReadOnlyList<RelacionEmpresaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<RelacionEmpresaDto?> ObtenerAsync(long idRelacionEmpresa, CancellationToken cancellationToken);
    Task<long> CrearAsync(RelacionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(RelacionEmpresaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idRelacionEmpresa, string? usuario, CancellationToken cancellationToken);
}
