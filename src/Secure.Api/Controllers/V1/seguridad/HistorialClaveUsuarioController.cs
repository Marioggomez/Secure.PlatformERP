using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.historial_clave_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/historial_clave_usuario")]
public sealed class HistorialClaveUsuarioController : ControllerBase
{
    private readonly IHistorialClaveUsuarioRepository _repository;

    public HistorialClaveUsuarioController(IHistorialClaveUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<HistorialClaveUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idHistorialClaveUsuario}")]
    [HttpGet("obtener/{idHistorialClaveUsuario}")]
    public async Task<ActionResult<HistorialClaveUsuarioDto>> ObtenerAsync([FromRoute] long idHistorialClaveUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idHistorialClaveUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] HistorialClaveUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idHistorialClaveUsuario}")]
    [HttpPut("actualizar/{idHistorialClaveUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idHistorialClaveUsuario, [FromBody] HistorialClaveUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdHistorialClaveUsuario = idHistorialClaveUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idHistorialClaveUsuario}")]
    [HttpDelete("desactivar/{idHistorialClaveUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idHistorialClaveUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idHistorialClaveUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



