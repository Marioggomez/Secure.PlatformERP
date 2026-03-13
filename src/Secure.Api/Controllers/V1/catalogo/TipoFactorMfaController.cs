using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_factor_mfa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_factor_mfa")]
public sealed class TipoFactorMfaController : ControllerBase
{
    private readonly ITipoFactorMfaRepository _repository;

    public TipoFactorMfaController(ITipoFactorMfaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoFactorMfaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoFactorMfa}")]
    [HttpGet("obtener/{idTipoFactorMfa}")]
    public async Task<ActionResult<TipoFactorMfaDto>> ObtenerAsync([FromRoute] short idTipoFactorMfa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoFactorMfa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoFactorMfaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoFactorMfa = id }, new { id });
    }

    [HttpPut("{idTipoFactorMfa}")]
    [HttpPut("actualizar/{idTipoFactorMfa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoFactorMfa, [FromBody] TipoFactorMfaDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoFactorMfa = idTipoFactorMfa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoFactorMfa}")]
    [HttpDelete("desactivar/{idTipoFactorMfa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoFactorMfa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoFactorMfa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


