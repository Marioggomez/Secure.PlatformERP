using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.sesion_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/sesion_usuario")]
public sealed class SesionUsuarioController : ControllerBase
{
    private readonly ISesionUsuarioRepository _repository;

    public SesionUsuarioController(ISesionUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SesionUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idSesionUsuario}")]
    public async Task<ActionResult<SesionUsuarioDto>> ObtenerAsync([FromRoute] Guid idSesionUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idSesionUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] SesionUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idSesionUsuario = id }, new { id });
    }

    [HttpPut("{idSesionUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] Guid idSesionUsuario, [FromBody] SesionUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdSesionUsuario = idSesionUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idSesionUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] Guid idSesionUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idSesionUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
