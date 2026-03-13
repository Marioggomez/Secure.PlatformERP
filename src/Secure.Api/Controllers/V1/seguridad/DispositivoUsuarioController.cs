using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.dispositivo_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/dispositivo_usuario")]
public sealed class DispositivoUsuarioController : ControllerBase
{
    private readonly IDispositivoUsuarioRepository _repository;

    public DispositivoUsuarioController(IDispositivoUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<DispositivoUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idDispositivoUsuario}")]
    [HttpGet("obtener/{idDispositivoUsuario}")]
    public async Task<ActionResult<DispositivoUsuarioDto>> ObtenerAsync([FromRoute] long idDispositivoUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idDispositivoUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] DispositivoUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idDispositivoUsuario = id }, new { id });
    }

    [HttpPut("{idDispositivoUsuario}")]
    [HttpPut("actualizar/{idDispositivoUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idDispositivoUsuario, [FromBody] DispositivoUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdDispositivoUsuario = idDispositivoUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idDispositivoUsuario}")]
    [HttpDelete("desactivar/{idDispositivoUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idDispositivoUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idDispositivoUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


