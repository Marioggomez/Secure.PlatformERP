using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;

namespace Secure.Platform.Api.Controllers.V1.Cumplimiento;

/// <summary>
/// Controller API v1 para la tabla cumplimiento.paso_perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/cumplimiento/paso_perfil_aprobacion")]
public sealed class PasoPerfilAprobacionController : ControllerBase
{
    private readonly IPasoPerfilAprobacionRepository _repository;

    public PasoPerfilAprobacionController(IPasoPerfilAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PasoPerfilAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPasoPerfilAprobacion}")]
    [HttpGet("obtener/{idPasoPerfilAprobacion}")]
    public async Task<ActionResult<PasoPerfilAprobacionDto>> ObtenerAsync([FromRoute] long idPasoPerfilAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPasoPerfilAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PasoPerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idPasoPerfilAprobacion}")]
    [HttpPut("actualizar/{idPasoPerfilAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPasoPerfilAprobacion, [FromBody] PasoPerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdPasoPerfilAprobacion = idPasoPerfilAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPasoPerfilAprobacion}")]
    [HttpDelete("desactivar/{idPasoPerfilAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPasoPerfilAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPasoPerfilAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



