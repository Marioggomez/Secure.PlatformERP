using Secure.Platform.Contracts.Dtos.Tercero;

namespace Secure.Platform.Data.Repositories.Interfaces.Tercero;

/// <summary>
/// Contrato del repositorio para tercero.contacto_tercero.
/// Autor: Mario Gomez.
/// </summary>
public interface IContactoTerceroRepository
{
    Task<IReadOnlyList<ContactoTerceroDto>> ListarAsync(CancellationToken cancellationToken);
    Task<ContactoTerceroDto?> ObtenerAsync(long idContactoTercero, CancellationToken cancellationToken);
    Task<long> CrearAsync(ContactoTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(ContactoTerceroDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idContactoTercero, string? usuario, CancellationToken cancellationToken);
}
