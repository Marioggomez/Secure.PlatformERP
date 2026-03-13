using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.excepcion_permiso_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/excepcion_permiso_usuario")]
public sealed class ExcepcionPermisoUsuarioController : ControllerBase
{
    private readonly IExcepcionPermisoUsuarioRepository _repository;

    public ExcepcionPermisoUsuarioController(IExcepcionPermisoUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ExcepcionPermisoUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idExcepcionPermisoUsuario}")]
    [HttpGet("obtener/{idExcepcionPermisoUsuario}")]
    public async Task<ActionResult<ExcepcionPermisoUsuarioDto>> ObtenerAsync([FromRoute] long idExcepcionPermisoUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idExcepcionPermisoUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idExcepcionPermisoUsuario}")]
    [HttpPut("actualizar/{idExcepcionPermisoUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idExcepcionPermisoUsuario, [FromBody] ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdExcepcionPermisoUsuario = idExcepcionPermisoUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idExcepcionPermisoUsuario}")]
    [HttpDelete("desactivar/{idExcepcionPermisoUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idExcepcionPermisoUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idExcepcionPermisoUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



