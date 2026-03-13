using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.asignacion_rol_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/asignacion_rol_usuario")]
public sealed class AsignacionRolUsuarioController : ControllerBase
{
    private readonly IAsignacionRolUsuarioRepository _repository;

    public AsignacionRolUsuarioController(IAsignacionRolUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<AsignacionRolUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAsignacionRolUsuario}")]
    [HttpGet("obtener/{idAsignacionRolUsuario}")]
    public async Task<ActionResult<AsignacionRolUsuarioDto>> ObtenerAsync([FromRoute] long idAsignacionRolUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAsignacionRolUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AsignacionRolUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idAsignacionRolUsuario = id }, new { id });
    }

    [HttpPut("{idAsignacionRolUsuario}")]
    [HttpPut("actualizar/{idAsignacionRolUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idAsignacionRolUsuario, [FromBody] AsignacionRolUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdAsignacionRolUsuario = idAsignacionRolUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAsignacionRolUsuario}")]
    [HttpDelete("desactivar/{idAsignacionRolUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idAsignacionRolUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAsignacionRolUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


