using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para persistencia de layout UI por usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/layout_ui")]
public sealed class LayoutUiController : ControllerBase
{
    private readonly ILayoutUiRepository _repository;

    public LayoutUiController(ILayoutUiRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<ObtenerLayoutUiResponseDto>> ObtenerAsync(
        [FromQuery] long idUsuario,
        [FromQuery] long idTenant,
        [FromQuery] long? idEmpresa,
        [FromQuery] string codigoLayout,
        CancellationToken cancellationToken)
    {
        var response = await _repository.ObtenerAsync(idUsuario, idTenant, idEmpresa, codigoLayout, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<GuardarLayoutUiResponseDto>> GuardarAsync(
        [FromBody] GuardarLayoutUiRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _repository.GuardarAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(response);
    }
}


