using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_evento_seguridad.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_evento_seguridad")]
public sealed class TipoEventoSeguridadController : ControllerBase
{
    private readonly ITipoEventoSeguridadRepository _repository;

    public TipoEventoSeguridadController(ITipoEventoSeguridadRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoEventoSeguridadDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoEventoSeguridad}")]
    [HttpGet("obtener/{idTipoEventoSeguridad}")]
    public async Task<ActionResult<TipoEventoSeguridadDto>> ObtenerAsync([FromRoute] short idTipoEventoSeguridad, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoEventoSeguridad, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idTipoEventoSeguridad}")]
    [HttpPut("actualizar/{idTipoEventoSeguridad}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoEventoSeguridad, [FromBody] TipoEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoEventoSeguridad = idTipoEventoSeguridad;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoEventoSeguridad}")]
    [HttpDelete("desactivar/{idTipoEventoSeguridad}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoEventoSeguridad, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoEventoSeguridad, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



