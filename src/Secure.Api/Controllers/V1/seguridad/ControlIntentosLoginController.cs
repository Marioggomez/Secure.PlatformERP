using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.control_intentos_login.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/control_intentos_login")]
public sealed class ControlIntentosLoginController : ControllerBase
{
    private readonly IControlIntentosLoginRepository _repository;

    public ControlIntentosLoginController(IControlIntentosLoginRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ControlIntentosLoginDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idControlIntento}")]
    [HttpGet("obtener/{idControlIntento}")]
    public async Task<ActionResult<ControlIntentosLoginDto>> ObtenerAsync([FromRoute] long idControlIntento, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idControlIntento, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ControlIntentosLoginDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idControlIntento = id }, new { id });
    }

    [HttpPut("{idControlIntento}")]
    [HttpPut("actualizar/{idControlIntento}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idControlIntento, [FromBody] ControlIntentosLoginDto dto, CancellationToken cancellationToken)
    {
        dto.IdControlIntento = idControlIntento;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idControlIntento}")]
    [HttpDelete("desactivar/{idControlIntento}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idControlIntento, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idControlIntento, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


