using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.contador_rate_limit.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/contador_rate_limit")]
public sealed class ContadorRateLimitController : ControllerBase
{
    private readonly IContadorRateLimitRepository _repository;

    public ContadorRateLimitController(IContadorRateLimitRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContadorRateLimitDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idContadorRateLimit}")]
    public async Task<ActionResult<ContadorRateLimitDto>> ObtenerAsync([FromRoute] long idContadorRateLimit, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idContadorRateLimit, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ContadorRateLimitDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idContadorRateLimit = id }, new { id });
    }

    [HttpPut("{idContadorRateLimit}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idContadorRateLimit, [FromBody] ContadorRateLimitDto dto, CancellationToken cancellationToken)
    {
        dto.IdContadorRateLimit = idContadorRateLimit;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idContadorRateLimit}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idContadorRateLimit, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idContadorRateLimit, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
