using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.accion_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/accion_instancia_aprobacion")]
public sealed class AccionInstanciaAprobacionController : ControllerBase
{
    private readonly IAccionInstanciaAprobacionRepository _repository;

    public AccionInstanciaAprobacionController(IAccionInstanciaAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<AccionInstanciaAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAccionInstanciaAprobacion}")]
    [HttpGet("obtener/{idAccionInstanciaAprobacion}")]
    public async Task<ActionResult<AccionInstanciaAprobacionDto>> ObtenerAsync([FromRoute] long idAccionInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAccionInstanciaAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAccionInstanciaAprobacion = id }, new { id });
    }

    [HttpPut("{idAccionInstanciaAprobacion}")]
    [HttpPut("actualizar/{idAccionInstanciaAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAccionInstanciaAprobacion, [FromBody] AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdAccionInstanciaAprobacion = idAccionInstanciaAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAccionInstanciaAprobacion}")]
    [HttpDelete("desactivar/{idAccionInstanciaAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAccionInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAccionInstanciaAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


