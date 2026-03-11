using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.proposito_desafio_mfa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/proposito_desafio_mfa")]
public sealed class PropositoDesafioMfaController : ControllerBase
{
    private readonly IPropositoDesafioMfaRepository _repository;

    public PropositoDesafioMfaController(IPropositoDesafioMfaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PropositoDesafioMfaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPropositoDesafioMfa}")]
    public async Task<ActionResult<PropositoDesafioMfaDto>> ObtenerAsync([FromRoute] short idPropositoDesafioMfa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPropositoDesafioMfa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PropositoDesafioMfaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idPropositoDesafioMfa = id }, new { id });
    }

    [HttpPut("{idPropositoDesafioMfa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idPropositoDesafioMfa, [FromBody] PropositoDesafioMfaDto dto, CancellationToken cancellationToken)
    {
        dto.IdPropositoDesafioMfa = idPropositoDesafioMfa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPropositoDesafioMfa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idPropositoDesafioMfa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPropositoDesafioMfa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
