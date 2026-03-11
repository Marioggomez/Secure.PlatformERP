using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.factor_mfa_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/factor_mfa_usuario")]
public sealed class FactorMfaUsuarioController : ControllerBase
{
    private readonly IFactorMfaUsuarioRepository _repository;

    public FactorMfaUsuarioController(IFactorMfaUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FactorMfaUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idFactorMfaUsuario}")]
    public async Task<ActionResult<FactorMfaUsuarioDto>> ObtenerAsync([FromRoute] long idFactorMfaUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idFactorMfaUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] FactorMfaUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idFactorMfaUsuario = id }, new { id });
    }

    [HttpPut("{idFactorMfaUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idFactorMfaUsuario, [FromBody] FactorMfaUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdFactorMfaUsuario = idFactorMfaUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idFactorMfaUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idFactorMfaUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idFactorMfaUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
