using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.flujo_autenticacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/flujo_autenticacion")]
public sealed class FlujoAutenticacionController : ControllerBase
{
    private readonly IFlujoAutenticacionRepository _repository;

    public FlujoAutenticacionController(IFlujoAutenticacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlujoAutenticacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idFlujoAutenticacion}")]
    public async Task<ActionResult<FlujoAutenticacionDto>> ObtenerAsync([FromRoute] Guid idFlujoAutenticacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idFlujoAutenticacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] FlujoAutenticacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idFlujoAutenticacion = id }, new { id });
    }

    [HttpPut("{idFlujoAutenticacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] Guid idFlujoAutenticacion, [FromBody] FlujoAutenticacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdFlujoAutenticacion = idFlujoAutenticacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idFlujoAutenticacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] Guid idFlujoAutenticacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idFlujoAutenticacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
