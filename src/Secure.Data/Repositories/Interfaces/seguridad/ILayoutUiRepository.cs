using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato para persistencia de layout UI por usuario.
/// Autor: Mario Gomez.
/// </summary>
public interface ILayoutUiRepository
{
    Task<ObtenerLayoutUiResponseDto> ObtenerAsync(long idUsuario, long idTenant, long? idEmpresa, string codigoLayout, CancellationToken cancellationToken);
    Task<GuardarLayoutUiResponseDto> GuardarAsync(GuardarLayoutUiRequestDto request, CancellationToken cancellationToken);
}
