using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.rol.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/rol")]
public sealed class RolController : ControllerBase
{
    private readonly IRolRepository _repository;

    public RolController(IRolRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<RolDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idRol}")]
    [HttpGet("obtener/{idRol}")]
    public async Task<ActionResult<RolDto>> ObtenerAsync([FromRoute] long idRol, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idRol, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] RolDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idRol}")]
    [HttpPut("actualizar/{idRol}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idRol, [FromBody] RolDto dto, CancellationToken cancellationToken)
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



