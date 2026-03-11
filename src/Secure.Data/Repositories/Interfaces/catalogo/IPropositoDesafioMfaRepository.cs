using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.proposito_desafio_mfa.
/// Autor: Mario Gomez.
/// </summary>
public interface IPropositoDesafioMfaRepository
{
    Task<IReadOnlyList<PropositoDesafioMfaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<PropositoDesafioMfaDto?> ObtenerAsync(short idPropositoDesafioMfa, CancellationToken cancellationToken);
    Task<short> CrearAsync(PropositoDesafioMfaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(PropositoDesafioMfaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idPropositoDesafioMfa, string? usuario, CancellationToken cancellationToken);
}
