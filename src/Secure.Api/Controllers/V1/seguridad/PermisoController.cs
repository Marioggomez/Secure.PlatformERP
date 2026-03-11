using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.permiso.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/permiso")]
public sealed class PermisoController : ControllerBase
{
    private readonly IPermisoRepository _repository;

    public PermisoController(IPermisoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PermisoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPermiso}")]
    public async Task<ActionResult<PermisoDto>> ObtenerAsync([FromRoute] int idPermiso, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPermiso, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PermisoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idPermiso = id }, new { id });
    }

    [HttpPut("{idPermiso}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idPermiso, [FromBody] PermisoDto dto, CancellationToken cancellationToken)
    {
        dto.IdPermiso = idPermiso;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPermiso}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idPermiso, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPermiso, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
