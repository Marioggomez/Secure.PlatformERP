using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/instancia_aprobacion")]
public sealed class InstanciaAprobacionController : ControllerBase
{
    private readonly IInstanciaAprobacionRepository _repository;

    public InstanciaAprobacionController(IInstanciaAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<InstanciaAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idInstanciaAprobacion}")]
    [HttpGet("obtener/{idInstanciaAprobacion}")]
    public async Task<ActionResult<InstanciaAprobacionDto>> ObtenerAsync([FromRoute] long idInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idInstanciaAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] InstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idInstanciaAprobacion = id }, new { id });
    }

    [HttpPut("{idInstanciaAprobacion}")]
    [HttpPut("actualizar/{idInstanciaAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idInstanciaAprobacion, [FromBody] InstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdInstanciaAprobacion = idInstanciaAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idInstanciaAprobacion}")]
    [HttpDelete("desactivar/{idInstanciaAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idInstanciaAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


