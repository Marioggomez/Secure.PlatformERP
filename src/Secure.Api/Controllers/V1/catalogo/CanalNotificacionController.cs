using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/canal_notificacion")]
public sealed class CanalNotificacionController : ControllerBase
{
    private readonly ICanalNotificacionRepository _repository;

    public CanalNotificacionController(ICanalNotificacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CanalNotificacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idCanalNotificacion}")]
    public async Task<ActionResult<CanalNotificacionDto>> ObtenerAsync([FromRoute] short idCanalNotificacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idCanalNotificacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] CanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idCanalNotificacion = id }, new { id });
    }

    [HttpPut("{idCanalNotificacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idCanalNotificacion, [FromBody] CanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdCanalNotificacion = idCanalNotificacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idCanalNotificacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idCanalNotificacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idCanalNotificacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
