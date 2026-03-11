using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.paso_instancia_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/paso_instancia_aprobacion")]
public sealed class PasoInstanciaAprobacionController : ControllerBase
{
    private readonly IPasoInstanciaAprobacionRepository _repository;

    public PasoInstanciaAprobacionController(IPasoInstanciaAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PasoInstanciaAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPasoInstanciaAprobacion}")]
    public async Task<ActionResult<PasoInstanciaAprobacionDto>> ObtenerAsync([FromRoute] long idPasoInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPasoInstanciaAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idPasoInstanciaAprobacion = id }, new { id });
    }

    [HttpPut("{idPasoInstanciaAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPasoInstanciaAprobacion, [FromBody] PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdPasoInstanciaAprobacion = idPasoInstanciaAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPasoInstanciaAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPasoInstanciaAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPasoInstanciaAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
