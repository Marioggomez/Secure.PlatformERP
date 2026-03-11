using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.evento_sistema.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/evento_sistema")]
public sealed class EventoSistemaController : ControllerBase
{
    private readonly IEventoSistemaRepository _repository;

    public EventoSistemaController(IEventoSistemaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventoSistemaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEventoSistema}")]
    public async Task<ActionResult<EventoSistemaDto>> ObtenerAsync([FromRoute] long idEventoSistema, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEventoSistema, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EventoSistemaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEventoSistema = id }, new { id });
    }

    [HttpPut("{idEventoSistema}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idEventoSistema, [FromBody] EventoSistemaDto dto, CancellationToken cancellationToken)
    {
        dto.IdEventoSistema = idEventoSistema;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEventoSistema}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idEventoSistema, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEventoSistema, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
