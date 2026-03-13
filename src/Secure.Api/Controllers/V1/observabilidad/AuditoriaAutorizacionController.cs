using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.auditoria_autorizacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/auditoria_autorizacion")]
public sealed class AuditoriaAutorizacionController : ControllerBase
{
    private readonly IAuditoriaAutorizacionRepository _repository;

    public AuditoriaAutorizacionController(IAuditoriaAutorizacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<AuditoriaAutorizacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAuditoriaAutorizacion}")]
    [HttpGet("obtener/{idAuditoriaAutorizacion}")]
    public async Task<ActionResult<AuditoriaAutorizacionDto>> ObtenerAsync([FromRoute] long idAuditoriaAutorizacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAuditoriaAutorizacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AuditoriaAutorizacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAuditoriaAutorizacion = id }, new { id });
    }

    [HttpPut("{idAuditoriaAutorizacion}")]
    [HttpPut("actualizar/{idAuditoriaAutorizacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAuditoriaAutorizacion, [FromBody] AuditoriaAutorizacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdAuditoriaAutorizacion = idAuditoriaAutorizacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAuditoriaAutorizacion}")]
    [HttpDelete("desactivar/{idAuditoriaAutorizacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAuditoriaAutorizacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAuditoriaAutorizacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


