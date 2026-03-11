using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.modo_filtro_dato.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/modo_filtro_dato")]
public sealed class ModoFiltroDatoController : ControllerBase
{
    private readonly IModoFiltroDatoRepository _repository;

    public ModoFiltroDatoController(IModoFiltroDatoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ModoFiltroDatoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idModoFiltroDato}")]
    public async Task<ActionResult<ModoFiltroDatoDto>> ObtenerAsync([FromRoute] short idModoFiltroDato, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idModoFiltroDato, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ModoFiltroDatoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idModoFiltroDato = id }, new { id });
    }

    [HttpPut("{idModoFiltroDato}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idModoFiltroDato, [FromBody] ModoFiltroDatoDto dto, CancellationToken cancellationToken)
    {
        dto.IdModoFiltroDato = idModoFiltroDato;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idModoFiltroDato}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idModoFiltroDato, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idModoFiltroDato, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
