using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.deber.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/deber")]
public sealed class DeberController : ControllerBase
{
    private readonly IDeberRepository _repository;

    public DeberController(IDeberRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<DeberDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idDeber}")]
    [HttpGet("obtener/{idDeber}")]
    public async Task<ActionResult<DeberDto>> ObtenerAsync([FromRoute] long idDeber, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idDeber, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] DeberDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idDeber}")]
    [HttpPut("actualizar/{idDeber}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idDeber, [FromBody] DeberDto dto, CancellationToken cancellationToken)
    {
        dto.IdDeber = idDeber;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idDeber}")]
    [HttpDelete("desactivar/{idDeber}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idDeber, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idDeber, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



