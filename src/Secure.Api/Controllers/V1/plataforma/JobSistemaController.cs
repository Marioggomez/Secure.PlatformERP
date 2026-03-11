using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.job_sistema.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/job_sistema")]
public sealed class JobSistemaController : ControllerBase
{
    private readonly IJobSistemaRepository _repository;

    public JobSistemaController(IJobSistemaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JobSistemaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idJob}")]
    public async Task<ActionResult<JobSistemaDto>> ObtenerAsync([FromRoute] long idJob, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idJob, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] JobSistemaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idJob = id }, new { id });
    }

    [HttpPut("{idJob}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idJob, [FromBody] JobSistemaDto dto, CancellationToken cancellationToken)
    {
        dto.IdJob = idJob;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idJob}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idJob, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idJob, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
