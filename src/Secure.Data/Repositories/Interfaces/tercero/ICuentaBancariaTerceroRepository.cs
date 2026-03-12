using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.cuenta_bancaria_tercero.
/// Autor: Mario Gomez.
/// </summary>
public interface ICuentaBancariaTerceroRepository
{
    Task<IReadOnlyList<CuentaBancariaTerceroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<CuentaBancariaTerceroDto?> ObtenerAsync(long idCuentaBancariaTercero, CancellationToken cancellationToken);
    Task<long> CrearAsync(CuentaBancariaTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(CuentaBancariaTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idCuentaBancariaTercero, string? usuario, CancellationToken cancellationToken);
}
