using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario")]
public sealed class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuarioController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("paginado")]
    [HttpGet("listar/paginado")]
    public async Task<ActionResult<PaginacionResultadoDto<UsuarioListadoDto>>> ListarPaginadoAsync([FromQuery] PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListarPaginadoAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuario}")]
    [HttpGet("obtener/{idUsuario}")]
    public async Task<ActionResult<UsuarioDto>> ObtenerAsync([FromRoute] long idUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idUsuario = id }, new { id });
    }

    [HttpPut("{idUsuario}")]
    [HttpPut("actualizar/{idUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuario, [FromBody] UsuarioDto dto, CancellationToken cancellationToken)
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


