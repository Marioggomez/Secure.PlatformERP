using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_recurso_ui.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_recurso_ui")]
public sealed class TipoRecursoUiController : ControllerBase
{
    private readonly ITipoRecursoUiRepository _repository;

    public TipoRecursoUiController(ITipoRecursoUiRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoRecursoUiDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoRecursoUi}")]
    [HttpGet("obtener/{idTipoRecursoUi}")]
    public async Task<ActionResult<TipoRecursoUiDto>> ObtenerAsync([FromRoute] short idTipoRecursoUi, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoRecursoUi, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoRecursoUiDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idTipoRecursoUi}")]
    [HttpPut("actualizar/{idTipoRecursoUi}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoRecursoUi, [FromBody] TipoRecursoUiDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoRecursoUi = idTipoRecursoUi;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoRecursoUi}")]
    [HttpDelete("desactivar/{idTipoRecursoUi}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoRecursoUi, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoRecursoUi, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



