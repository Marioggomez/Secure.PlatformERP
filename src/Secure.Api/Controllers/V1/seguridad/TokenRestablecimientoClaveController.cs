using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.token_restablecimiento_clave.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/token_restablecimiento_clave")]
public sealed class TokenRestablecimientoClaveController : ControllerBase
{
    private readonly ITokenRestablecimientoClaveRepository _repository;

    public TokenRestablecimientoClaveController(ITokenRestablecimientoClaveRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TokenRestablecimientoClaveDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTokenRestablecimientoClave}")]
    public async Task<ActionResult<TokenRestablecimientoClaveDto>> ObtenerAsync([FromRoute] Guid idTokenRestablecimientoClave, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTokenRestablecimientoClave, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TokenRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTokenRestablecimientoClave = id }, new { id });
    }

    [HttpPut("{idTokenRestablecimientoClave}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] Guid idTokenRestablecimientoClave, [FromBody] TokenRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        dto.IdTokenRestablecimientoClave = idTokenRestablecimientoClave;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTokenRestablecimientoClave}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] Guid idTokenRestablecimientoClave, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTokenRestablecimientoClave, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
