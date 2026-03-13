using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.regla_sod.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/regla_sod")]
public sealed class ReglaSodController : ControllerBase
{
    private readonly IReglaSodRepository _repository;

    public ReglaSodController(IReglaSodRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ReglaSodDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idReglaSod}")]
    [HttpGet("obtener/{idReglaSod}")]
    public async Task<ActionResult<ReglaSodDto>> ObtenerAsync([FromRoute] long idReglaSod, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idReglaSod, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ReglaSodDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idReglaSod = id }, new { id });
    }

    [HttpPut("{idReglaSod}")]
    [HttpPut("actualizar/{idReglaSod}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idReglaSod, [FromBody] ReglaSodDto dto, CancellationToken cancellationToken)
    {
        dto.IdReglaSod = idReglaSod;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idReglaSod}")]
    [HttpDelete("desactivar/{idReglaSod}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idReglaSod, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idReglaSod, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


