using Secure.Platform.Contracts.Dtos.Seguridad;

namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Contrato del servicio IAM para login, MFA y restablecimiento.
/// Autor: Mario Gomez.
/// </summary>
public interface IAutenticacionIamService
{
    Task<LoginResponseDto> IniciarLoginAsync(LoginRequestDto request, CancellationToken cancellationToken);

    Task<ValidarMfaResponseDto> ValidarMfaAsync(ValidarMfaRequestDto request, CancellationToken cancellationToken);

    Task<ReenviarMfaResponseDto> ReenviarMfaAsync(ReenviarMfaRequestDto request, CancellationToken cancellationToken);

    Task<SeleccionarEmpresaResponseDto> SeleccionarEmpresaAsync(SeleccionarEmpresaRequestDto request, CancellationToken cancellationToken);

    Task<IniciarRestablecimientoClaveResponseDto> IniciarRestablecimientoAsync(IniciarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken);

    Task<CompletarRestablecimientoClaveResponseDto> CompletarRestablecimientoAsync(CompletarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken);
}
