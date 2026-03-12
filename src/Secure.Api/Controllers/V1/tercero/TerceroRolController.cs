using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.tercero_rol.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/tercero_rol")]
public sealed class TerceroRolController : ControllerBase
{
    private readonly ITerceroRolRepository _repository;

    public TerceroRolController(ITerceroRolRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TerceroRolDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTerceroRol}")]
    public async Task<ActionResult<TerceroRolDto>> ObtenerAsync([FromRoute] long idTerceroRol, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTerceroRol, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TerceroRolDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTerceroRol = id }, new { id });
    }

    [HttpPut("{idTerceroRol}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idTerceroRol, [FromBody] TerceroRolDto dto, CancellationToken cancellationToken)
    {
        dto.IdTerceroRol = idTerceroRol;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTerceroRol}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idTerceroRol, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTerceroRol, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
