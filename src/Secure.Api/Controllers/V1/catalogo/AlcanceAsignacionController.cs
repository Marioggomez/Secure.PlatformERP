using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.alcance_asignacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/alcance_asignacion")]
public sealed class AlcanceAsignacionController : ControllerBase
{
    private readonly IAlcanceAsignacionRepository _repository;

    public AlcanceAsignacionController(IAlcanceAsignacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<AlcanceAsignacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idAlcanceAsignacion}")]
    [HttpGet("obtener/{idAlcanceAsignacion}")]
    public async Task<ActionResult<AlcanceAsignacionDto>> ObtenerAsync([FromRoute] short idAlcanceAsignacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idAlcanceAsignacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] AlcanceAsignacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idAlcanceAsignacion}")]
    [HttpPut("actualizar/{idAlcanceAsignacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idAlcanceAsignacion, [FromBody] AlcanceAsignacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdAlcanceAsignacion = idAlcanceAsignacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idAlcanceAsignacion}")]
    [HttpDelete("desactivar/{idAlcanceAsignacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idAlcanceAsignacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idAlcanceAsignacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}

