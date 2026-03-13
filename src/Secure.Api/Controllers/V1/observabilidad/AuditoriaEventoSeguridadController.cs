using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.auditoria_evento_seguridad.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/auditoria_evento_seguridad")]
public sealed class AuditoriaEventoSeguridadController : ControllerBase
{
    private readonly IAuditoriaEventoSeguridadRepository _repository;

    public AuditoriaEventoSeguridadController(IAuditoriaEventoSeguridadRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<AuditoriaEventoSeguridadDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAuditoriaEventoSeguridad}")]
    [HttpGet("obtener/{idAuditoriaEventoSeguridad}")]
    public async Task<ActionResult<AuditoriaEventoSeguridadDto>> ObtenerAsync([FromRoute] long idAuditoriaEventoSeguridad, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAuditoriaEventoSeguridad, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAuditoriaEventoSeguridad = id }, new { id });
    }

    [HttpPut("{idAuditoriaEventoSeguridad}")]
    [HttpPut("actualizar/{idAuditoriaEventoSeguridad}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAuditoriaEventoSeguridad, [FromBody] AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        dto.IdAuditoriaEventoSeguridad = idAuditoriaEventoSeguridad;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAuditoriaEventoSeguridad}")]
    [HttpDelete("desactivar/{idAuditoriaEventoSeguridad}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAuditoriaEventoSeguridad, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAuditoriaEventoSeguridad, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


