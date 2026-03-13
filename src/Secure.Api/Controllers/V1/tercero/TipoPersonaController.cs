using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.tipo_persona.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/tipo_persona")]
public sealed class TipoPersonaController : ControllerBase
{
    private readonly ITipoPersonaRepository _repository;

    public TipoPersonaController(ITipoPersonaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoPersonaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoPersona}")]
    [HttpGet("obtener/{idTipoPersona}")]
    public async Task<ActionResult<TipoPersonaDto>> ObtenerAsync([FromRoute] int idTipoPersona, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoPersona, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoPersonaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoPersona = id }, new { id });
    }

    [HttpPut("{idTipoPersona}")]
    [HttpPut("actualizar/{idTipoPersona}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idTipoPersona, [FromBody] TipoPersonaDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoPersona = idTipoPersona;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoPersona}")]
    [HttpDelete("desactivar/{idTipoPersona}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idTipoPersona, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoPersona, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


