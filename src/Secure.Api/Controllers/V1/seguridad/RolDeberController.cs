using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.rol_deber.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/rol_deber")]
public sealed class RolDeberController : ControllerBase
{
    private readonly IRolDeberRepository _repository;

    public RolDeberController(IRolDeberRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<RolDeberDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idRol}")]
    [HttpGet("obtener/{idRol}")]
    public async Task<ActionResult<RolDeberDto>> ObtenerAsync([FromRoute] long idRol, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idRol, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] RolDeberDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idRol}")]
    [HttpPut("actualizar/{idRol}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idRol, [FromBody] RolDeberDto dto, CancellationToken cancellationToken)
    {
        dto.IdRol = idRol;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idRol}")]
    [HttpDelete("desactivar/{idRol}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idRol, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idRol, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



