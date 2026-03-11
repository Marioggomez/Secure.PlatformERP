using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.accion_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/accion_aprobacion")]
public sealed class AccionAprobacionController : ControllerBase
{
    private readonly IAccionAprobacionRepository _repository;

    public AccionAprobacionController(IAccionAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AccionAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAccionAprobacion}")]
    public async Task<ActionResult<AccionAprobacionDto>> ObtenerAsync([FromRoute] short idAccionAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAccionAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AccionAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAccionAprobacion = id }, new { id });
    }

    [HttpPut("{idAccionAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idAccionAprobacion, [FromBody] AccionAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdAccionAprobacion = idAccionAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAccionAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idAccionAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAccionAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
