using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.error_log.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/error_log")]
public sealed class ErrorLogController : ControllerBase
{
    private readonly IErrorLogRepository _repository;

    public ErrorLogController(IErrorLogRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ErrorLogDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idErrorLog}")]
    [HttpGet("obtener/{idErrorLog}")]
    public async Task<ActionResult<ErrorLogDto>> ObtenerAsync([FromRoute] long idErrorLog, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idErrorLog, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ErrorLogDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idErrorLog}")]
    [HttpPut("actualizar/{idErrorLog}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idErrorLog, [FromBody] ErrorLogDto dto, CancellationToken cancellationToken)
    {
        dto.IdErrorLog = idErrorLog;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idErrorLog}")]
    [HttpDelete("desactivar/{idErrorLog}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idErrorLog, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idErrorLog, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



