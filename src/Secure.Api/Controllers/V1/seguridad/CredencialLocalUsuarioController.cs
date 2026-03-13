using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.credencial_local_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/credencial_local_usuario")]
public sealed class CredencialLocalUsuarioController : ControllerBase
{
    private readonly ICredencialLocalUsuarioRepository _repository;

    public CredencialLocalUsuarioController(ICredencialLocalUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<CredencialLocalUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuario}")]
    [HttpGet("obtener/{idUsuario}")]
    public async Task<ActionResult<CredencialLocalUsuarioDto>> ObtenerAsync([FromRoute] long idUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] CredencialLocalUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idUsuario}")]
    [HttpPut("actualizar/{idUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuario, [FromBody] CredencialLocalUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuario = idUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuario}")]
    [HttpDelete("desactivar/{idUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



