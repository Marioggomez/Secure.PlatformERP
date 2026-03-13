using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Infrastructure.Security;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Endpoints IAM para autenticacion, MFA y restablecimiento de clave.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad")]
public sealed class AutenticacionController : ControllerBase
{
    private readonly IAutenticacionIamService _service;

    public AutenticacionController(IAutenticacionIamService service)
    {
        _service = service;
    }

    /// <summary>
    /// Inicia el flujo de autenticacion primaria.
    /// </summary>
    [HttpPost("flujo_autenticacion/iniciar")]
    public async Task<ActionResult<LoginResponseDto>> IniciarFlujoAutenticacionAsync([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        request.IpOrigen ??= HttpContext.Connection.RemoteIpAddress?.ToString();
        request.AgenteUsuario ??= Request.Headers.UserAgent.ToString();

        var response = await _service.IniciarLoginAsync(request, cancellationToken).ConfigureAwait(false);
        return response.Autenticado ? Ok(response) : Unauthorized(response);
    }

    /// <summary>
    /// Valida el codigo MFA para cerrar el flujo IAM.
    /// </summary>
    [HttpPost("desafio_mfa/validar")]
    public async Task<ActionResult<ValidarMfaResponseDto>> ValidarMfaAsync([FromBody] ValidarMfaRequestDto request, CancellationToken cancellationToken)
    {
        request.IpOrigen ??= HttpContext.Connection.RemoteIpAddress?.ToString();
        request.AgenteUsuario ??= Request.Headers.UserAgent.ToString();

        var response = await _service.ValidarMfaAsync(request, cancellationToken).ConfigureAwait(false);
        return response.Validado ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Reenvia y regenera un desafio MFA vigente para el flujo indicado.
    /// </summary>
    [HttpPost("desafio_mfa/reenviar")]
    public async Task<ActionResult<ReenviarMfaResponseDto>> ReenviarMfaAsync([FromBody] ReenviarMfaRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _service.ReenviarMfaAsync(request, cancellationToken).ConfigureAwait(false);
        return response.Reenviado ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Selecciona la empresa de trabajo para finalizar sesion IAM.
    /// </summary>
    [HttpPost("flujo_autenticacion/seleccionar_empresa")]
    public async Task<ActionResult<SeleccionarEmpresaResponseDto>> SeleccionarEmpresaAsync([FromBody] SeleccionarEmpresaRequestDto request, CancellationToken cancellationToken)
    {
        request.IpOrigen ??= HttpContext.Connection.RemoteIpAddress?.ToString();
        request.AgenteUsuario ??= Request.Headers.UserAgent.ToString();

        var response = await _service.SeleccionarEmpresaAsync(request, cancellationToken).ConfigureAwait(false);
        return response.SeleccionAplicada ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Inicia un flujo de recuperacion de contrasena.
    /// </summary>
    [HttpPost("flujo_restablecimiento_clave/iniciar")]
    public async Task<ActionResult<IniciarRestablecimientoClaveResponseDto>> IniciarRestablecimientoAsync([FromBody] IniciarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken)
    {
        request.IpOrigen ??= HttpContext.Connection.RemoteIpAddress?.ToString();
        request.AgenteUsuario ??= Request.Headers.UserAgent.ToString();

        var response = await _service.IniciarRestablecimientoAsync(request, cancellationToken).ConfigureAwait(false);
        return response.Iniciado ? Ok(response) : BadRequest(response);
    }

    /// <summary>
    /// Completa el restablecimiento de contrasena con token vigente.
    /// </summary>
    [HttpPost("flujo_restablecimiento_clave/completar")]
    public async Task<ActionResult<CompletarRestablecimientoClaveResponseDto>> CompletarRestablecimientoAsync([FromBody] CompletarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken)
    {
        var response = await _service.CompletarRestablecimientoAsync(request, cancellationToken).ConfigureAwait(false);
        return response.Restablecido ? Ok(response) : BadRequest(response);
    }
}
