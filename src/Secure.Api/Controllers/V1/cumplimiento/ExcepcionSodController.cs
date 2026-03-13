using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.excepcion_sod.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/excepcion_sod")]
public sealed class ExcepcionSodController : ControllerBase
{
    private readonly IExcepcionSodRepository _repository;

    public ExcepcionSodController(IExcepcionSodRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ExcepcionSodDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idExcepcionSod}")]
    [HttpGet("obtener/{idExcepcionSod}")]
    public async Task<ActionResult<ExcepcionSodDto>> ObtenerAsync([FromRoute] long idExcepcionSod, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idExcepcionSod, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ExcepcionSodDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idExcepcionSod = id }, new { id });
    }

    [HttpPut("{idExcepcionSod}")]
    [HttpPut("actualizar/{idExcepcionSod}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idExcepcionSod, [FromBody] ExcepcionSodDto dto, CancellationToken cancellationToken)
    {
        dto.IdExcepcionSod = idExcepcionSod;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idExcepcionSod}")]
    [HttpDelete("desactivar/{idExcepcionSod}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idExcepcionSod, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idExcepcionSod, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


