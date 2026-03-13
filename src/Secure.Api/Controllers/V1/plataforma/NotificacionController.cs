using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.notificacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/notificacion")]
public sealed class NotificacionController : ControllerBase
{
    private readonly INotificacionRepository _repository;

    public NotificacionController(INotificacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<NotificacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idNotificacion}")]
    [HttpGet("obtener/{idNotificacion}")]
    public async Task<ActionResult<NotificacionDto>> ObtenerAsync([FromRoute] long idNotificacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idNotificacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] NotificacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idNotificacion = id }, new { id });
    }

    [HttpPut("{idNotificacion}")]
    [HttpPut("actualizar/{idNotificacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idNotificacion, [FromBody] NotificacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdNotificacion = idNotificacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idNotificacion}")]
    [HttpDelete("desactivar/{idNotificacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idNotificacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idNotificacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


