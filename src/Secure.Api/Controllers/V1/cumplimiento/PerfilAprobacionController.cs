using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/perfil_aprobacion")]
public sealed class PerfilAprobacionController : ControllerBase
{
    private readonly IPerfilAprobacionRepository _repository;

    public PerfilAprobacionController(IPerfilAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PerfilAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPerfilAprobacion}")]
    [HttpGet("obtener/{idPerfilAprobacion}")]
    public async Task<ActionResult<PerfilAprobacionDto>> ObtenerAsync([FromRoute] long idPerfilAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPerfilAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idPerfilAprobacion}")]
    [HttpPut("actualizar/{idPerfilAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPerfilAprobacion, [FromBody] PerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdPerfilAprobacion = idPerfilAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPerfilAprobacion}")]
    [HttpDelete("desactivar/{idPerfilAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPerfilAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPerfilAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



