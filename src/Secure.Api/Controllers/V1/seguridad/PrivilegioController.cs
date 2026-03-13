using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.privilegio.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/privilegio")]
public sealed class PrivilegioController : ControllerBase
{
    private readonly IPrivilegioRepository _repository;

    public PrivilegioController(IPrivilegioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PrivilegioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPrivilegio}")]
    [HttpGet("obtener/{idPrivilegio}")]
    public async Task<ActionResult<PrivilegioDto>> ObtenerAsync([FromRoute] long idPrivilegio, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPrivilegio, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PrivilegioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idPrivilegio}")]
    [HttpPut("actualizar/{idPrivilegio}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPrivilegio, [FromBody] PrivilegioDto dto, CancellationToken cancellationToken)
    {
        dto.IdPrivilegio = idPrivilegio;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPrivilegio}")]
    [HttpDelete("desactivar/{idPrivilegio}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPrivilegio, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPrivilegio, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



