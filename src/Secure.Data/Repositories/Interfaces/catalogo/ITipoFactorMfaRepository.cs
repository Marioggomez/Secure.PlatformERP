using Secure.Platform.Contracts.Dtos.Catalogo;

namespace Secure.Platform.Data.Repositories.Interfaces.Catalogo;

/// <summary>
/// Contrato del repositorio para catalogo.tipo_factor_mfa.
/// Autor: Mario Gomez.
/// </summary>
public interface ITipoFactorMfaRepository
{
    Task<IReadOnlyList<TipoFactorMfaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<TipoFactorMfaDto?> ObtenerAsync(short idTipoFactorMfa, CancellationToken cancellationToken);
    Task<short> CrearAsync(TipoFactorMfaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(TipoFactorMfaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(short idTipoFactorMfa, string? usuario, CancellationToken cancellationToken);
}
