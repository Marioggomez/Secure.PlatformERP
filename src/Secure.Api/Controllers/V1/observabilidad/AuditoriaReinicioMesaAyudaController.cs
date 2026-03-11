using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

namespace Secure.Platform.Api.Controllers.V1.Observabilidad;

/// <summary>
/// Controller API v1 para la tabla observabilidad.auditoria_reinicio_mesa_ayuda.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/observabilidad/auditoria_reinicio_mesa_ayuda")]
public sealed class AuditoriaReinicioMesaAyudaController : ControllerBase
{
    private readonly IAuditoriaReinicioMesaAyudaRepository _repository;

    public AuditoriaReinicioMesaAyudaController(IAuditoriaReinicioMesaAyudaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditoriaReinicioMesaAyudaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAuditoriaReinicioMesaAyuda}")]
    public async Task<ActionResult<AuditoriaReinicioMesaAyudaDto>> ObtenerAsync([FromRoute] long idAuditoriaReinicioMesaAyuda, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAuditoriaReinicioMesaAyuda, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAuditoriaReinicioMesaAyuda = id }, new { id });
    }

    [HttpPut("{idAuditoriaReinicioMesaAyuda}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAuditoriaReinicioMesaAyuda, [FromBody] AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken)
    {
        dto.IdAuditoriaReinicioMesaAyuda = idAuditoriaReinicioMesaAyuda;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAuditoriaReinicioMesaAyuda}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAuditoriaReinicioMesaAyuda, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAuditoriaReinicioMesaAyuda, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
