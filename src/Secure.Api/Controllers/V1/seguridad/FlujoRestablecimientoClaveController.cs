using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.flujo_restablecimiento_clave.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/flujo_restablecimiento_clave")]
public sealed class FlujoRestablecimientoClaveController : ControllerBase
{
    private readonly IFlujoRestablecimientoClaveRepository _repository;

    public FlujoRestablecimientoClaveController(IFlujoRestablecimientoClaveRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<FlujoRestablecimientoClaveDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idFlujoRestablecimientoClave}")]
    [HttpGet("obtener/{idFlujoRestablecimientoClave}")]
    public async Task<ActionResult<FlujoRestablecimientoClaveDto>> ObtenerAsync([FromRoute] Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idFlujoRestablecimientoClave, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idFlujoRestablecimientoClave}")]
    [HttpPut("actualizar/{idFlujoRestablecimientoClave}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] Guid idFlujoRestablecimientoClave, [FromBody] FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        dto.IdFlujoRestablecimientoClave = idFlujoRestablecimientoClave;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idFlujoRestablecimientoClave}")]
    [HttpDelete("desactivar/{idFlujoRestablecimientoClave}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idFlujoRestablecimientoClave, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



