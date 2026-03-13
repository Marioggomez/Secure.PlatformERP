using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.desafio_mfa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/desafio_mfa")]
public sealed class DesafioMfaController : ControllerBase
{
    private readonly IDesafioMfaRepository _repository;

    public DesafioMfaController(IDesafioMfaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<DesafioMfaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idDesafioMfa}")]
    [HttpGet("obtener/{idDesafioMfa}")]
    public async Task<ActionResult<DesafioMfaDto>> ObtenerAsync([FromRoute] Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idDesafioMfa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] DesafioMfaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idDesafioMfa = id }, new { id });
    }

    [HttpPut("{idDesafioMfa}")]
    [HttpPut("actualizar/{idDesafioMfa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] Guid idDesafioMfa, [FromBody] DesafioMfaDto dto, CancellationToken cancellationToken)
    {
        dto.IdDesafioMfa = idDesafioMfa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idDesafioMfa}")]
    [HttpDelete("desactivar/{idDesafioMfa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idDesafioMfa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


