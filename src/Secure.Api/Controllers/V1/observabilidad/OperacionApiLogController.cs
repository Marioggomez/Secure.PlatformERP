using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.operacion_api_log.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/operacion_api_log")]
public sealed class OperacionApiLogController : ControllerBase
{
    private readonly IOperacionApiLogRepository _repository;

    public OperacionApiLogController(IOperacionApiLogRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<OperacionApiLogDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idOperacionApiLog}")]
    [HttpGet("obtener/{idOperacionApiLog}")]
    public async Task<ActionResult<OperacionApiLogDto>> ObtenerAsync([FromRoute] long idOperacionApiLog, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idOperacionApiLog, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] OperacionApiLogDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idOperacionApiLog}")]
    [HttpPut("actualizar/{idOperacionApiLog}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idOperacionApiLog, [FromBody] OperacionApiLogDto dto, CancellationToken cancellationToken)
    {
        dto.IdOperacionApiLog = idOperacionApiLog;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idOperacionApiLog}")]
    [HttpDelete("desactivar/{idOperacionApiLog}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idOperacionApiLog, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idOperacionApiLog, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



