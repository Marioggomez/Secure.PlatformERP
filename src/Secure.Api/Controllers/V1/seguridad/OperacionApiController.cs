using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.operacion_api.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/operacion_api")]
public sealed class OperacionApiController : ControllerBase
{
    private readonly IOperacionApiRepository _repository;

    public OperacionApiController(IOperacionApiRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<OperacionApiDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idOperacionApi}")]
    [HttpGet("obtener/{idOperacionApi}")]
    public async Task<ActionResult<OperacionApiDto>> ObtenerAsync([FromRoute] long idOperacionApi, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idOperacionApi, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] OperacionApiDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idOperacionApi = id }, new { id });
    }

    [HttpPut("{idOperacionApi}")]
    [HttpPut("actualizar/{idOperacionApi}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idOperacionApi, [FromBody] OperacionApiDto dto, CancellationToken cancellationToken)
    {
        dto.IdOperacionApi = idOperacionApi;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idOperacionApi}")]
    [HttpDelete("desactivar/{idOperacionApi}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idOperacionApi, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idOperacionApi, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


