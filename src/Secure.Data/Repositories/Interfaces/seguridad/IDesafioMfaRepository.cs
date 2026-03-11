using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato del repositorio para seguridad.desafio_mfa.
/// Autor: Mario Gomez.
/// </summary>
public interface IDesafioMfaRepository
{
    Task<IReadOnlyList<DesafioMfaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<DesafioMfaDto?> ObtenerAsync(Guid idDesafioMfa, CancellationToken cancellationToken);
    Task<Guid> CrearAsync(DesafioMfaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(DesafioMfaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(Guid idDesafioMfa, string? usuario, CancellationToken cancellationToken);
}
