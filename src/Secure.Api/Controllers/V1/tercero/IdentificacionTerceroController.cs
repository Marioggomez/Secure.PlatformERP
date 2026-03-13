using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.identificacion_tercero.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/identificacion_tercero")]
public sealed class IdentificacionTerceroController : ControllerBase
{
    private readonly IIdentificacionTerceroRepository _repository;

    public IdentificacionTerceroController(IIdentificacionTerceroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<IdentificacionTerceroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idIdentificacionTercero}")]
    [HttpGet("obtener/{idIdentificacionTercero}")]
    public async Task<ActionResult<IdentificacionTerceroDto>> ObtenerAsync([FromRoute] long idIdentificacionTercero, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idIdentificacionTercero, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] IdentificacionTerceroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idIdentificacionTercero = id }, new { id });
    }

    [HttpPut("{idIdentificacionTercero}")]
    [HttpPut("actualizar/{idIdentificacionTercero}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idIdentificacionTercero, [FromBody] IdentificacionTerceroDto dto, CancellationToken cancellationToken)
    {
        dto.IdIdentificacionTercero = idIdentificacionTercero;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idIdentificacionTercero}")]
    [HttpDelete("desactivar/{idIdentificacionTercero}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idIdentificacionTercero, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idIdentificacionTercero, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


