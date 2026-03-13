using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.error_aplicacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/error_aplicacion")]
public sealed class ErrorAplicacionController : ControllerBase
{
    private readonly IErrorAplicacionRepository _repository;

    public ErrorAplicacionController(IErrorAplicacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ErrorAplicacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idErrorAplicacion}")]
    [HttpGet("obtener/{idErrorAplicacion}")]
    public async Task<ActionResult<ErrorAplicacionDto>> ObtenerAsync([FromRoute] long idErrorAplicacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idErrorAplicacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ErrorAplicacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idErrorAplicacion}")]
    [HttpPut("actualizar/{idErrorAplicacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idErrorAplicacion, [FromBody] ErrorAplicacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdErrorAplicacion = idErrorAplicacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idErrorAplicacion}")]
    [HttpDelete("desactivar/{idErrorAplicacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idErrorAplicacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idErrorAplicacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



