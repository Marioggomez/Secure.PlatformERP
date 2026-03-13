using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.estado_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/estado_aprobacion")]
public sealed class EstadoAprobacionController : ControllerBase
{
    private readonly IEstadoAprobacionRepository _repository;

    public EstadoAprobacionController(IEstadoAprobacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EstadoAprobacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEstadoAprobacion}")]
    [HttpGet("obtener/{idEstadoAprobacion}")]
    public async Task<ActionResult<EstadoAprobacionDto>> ObtenerAsync([FromRoute] short idEstadoAprobacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEstadoAprobacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EstadoAprobacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEstadoAprobacion = id }, new { id });
    }

    [HttpPut("{idEstadoAprobacion}")]
    [HttpPut("actualizar/{idEstadoAprobacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idEstadoAprobacion, [FromBody] EstadoAprobacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdEstadoAprobacion = idEstadoAprobacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEstadoAprobacion}")]
    [HttpDelete("desactivar/{idEstadoAprobacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idEstadoAprobacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEstadoAprobacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


