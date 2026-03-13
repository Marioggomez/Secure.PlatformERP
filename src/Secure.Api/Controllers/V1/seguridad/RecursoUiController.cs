using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.recurso_ui.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/recurso_ui")]
public sealed class RecursoUiController : ControllerBase
{
    private readonly IRecursoUiRepository _repository;

    public RecursoUiController(IRecursoUiRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<RecursoUiDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idRecursoUi}")]
    [HttpGet("obtener/{idRecursoUi}")]
    public async Task<ActionResult<RecursoUiDto>> ObtenerAsync([FromRoute] long idRecursoUi, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idRecursoUi, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] RecursoUiDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idRecursoUi = id }, new { id });
    }

    [HttpPut("{idRecursoUi}")]
    [HttpPut("actualizar/{idRecursoUi}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idRecursoUi, [FromBody] RecursoUiDto dto, CancellationToken cancellationToken)
    {
        dto.IdRecursoUi = idRecursoUi;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idRecursoUi}")]
    [HttpDelete("desactivar/{idRecursoUi}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idRecursoUi, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idRecursoUi, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


