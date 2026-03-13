using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.job_sistema_ejecucion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/job_sistema_ejecucion")]
public sealed class JobSistemaEjecucionController : ControllerBase
{
    private readonly IJobSistemaEjecucionRepository _repository;

    public JobSistemaEjecucionController(IJobSistemaEjecucionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<JobSistemaEjecucionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEjecucion}")]
    [HttpGet("obtener/{idEjecucion}")]
    public async Task<ActionResult<JobSistemaEjecucionDto>> ObtenerAsync([FromRoute] long idEjecucion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEjecucion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] JobSistemaEjecucionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEjecucion = id }, new { id });
    }

    [HttpPut("{idEjecucion}")]
    [HttpPut("actualizar/{idEjecucion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idEjecucion, [FromBody] JobSistemaEjecucionDto dto, CancellationToken cancellationToken)
    {
        dto.IdEjecucion = idEjecucion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEjecucion}")]
    [HttpDelete("desactivar/{idEjecucion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idEjecucion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEjecucion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


