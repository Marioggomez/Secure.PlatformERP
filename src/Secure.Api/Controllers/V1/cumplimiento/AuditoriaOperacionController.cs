using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.auditoria_operacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/auditoria_operacion")]
public sealed class AuditoriaOperacionController : ControllerBase
{
    private readonly IAuditoriaOperacionRepository _repository;

    public AuditoriaOperacionController(IAuditoriaOperacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AuditoriaOperacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAuditoria}")]
    public async Task<ActionResult<AuditoriaOperacionDto>> ObtenerAsync([FromRoute] long idAuditoria, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAuditoria, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AuditoriaOperacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAuditoria = id }, new { id });
    }

    [HttpPut("{idAuditoria}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAuditoria, [FromBody] AuditoriaOperacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdAuditoria = idAuditoria;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAuditoria}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAuditoria, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAuditoria, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
