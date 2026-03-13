using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.sesion_usuario_historial.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/sesion_usuario_historial")]
public sealed class SesionUsuarioHistorialController : ControllerBase
{
    private readonly ISesionUsuarioHistorialRepository _repository;

    public SesionUsuarioHistorialController(ISesionUsuarioHistorialRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<SesionUsuarioHistorialDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idHistorial}")]
    [HttpGet("obtener/{idHistorial}")]
    public async Task<ActionResult<SesionUsuarioHistorialDto>> ObtenerAsync([FromRoute] long idHistorial, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idHistorial, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] SesionUsuarioHistorialDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idHistorial = id }, new { id });
    }

    [HttpPut("{idHistorial}")]
    [HttpPut("actualizar/{idHistorial}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idHistorial, [FromBody] SesionUsuarioHistorialDto dto, CancellationToken cancellationToken)
    {
        dto.IdHistorial = idHistorial;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idHistorial}")]
    [HttpDelete("desactivar/{idHistorial}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idHistorial, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idHistorial, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


