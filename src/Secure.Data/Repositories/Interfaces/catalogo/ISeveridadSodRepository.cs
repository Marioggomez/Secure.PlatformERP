using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.severidad_sod.
/// Autor: Mario Gomez.
/// </summary>
public interface ISeveridadSodRepository
{
    Task<IReadOnlyList<SeveridadSodDto>> ListarAsync(CancellationToken cancellationToken);
    Task<SeveridadSodDto?> ObtenerAsync(short idSeveridadSod, CancellationToken cancellationToken);
    Task<short> CrearAsync(SeveridadSodDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(SeveridadSodDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idSeveridadSod, string? usuario, CancellationToken cancellationToken);
}
