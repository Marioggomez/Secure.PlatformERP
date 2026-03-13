using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.tercero.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/tercero")]
public sealed class TerceroController : ControllerBase
{
    private readonly ITerceroRepository _repository;

    public TerceroController(ITerceroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TerceroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("paginado")]
    [HttpGet("listar/paginado")]
    public async Task<ActionResult<PaginacionResultadoDto<TerceroListadoDto>>> ListarPaginadoAsync([FromQuery] PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListarPaginadoAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTercero}")]
    [HttpGet("obtener/{idTercero}")]
    public async Task<ActionResult<TerceroDto>> ObtenerAsync([FromRoute] long idTercero, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTercero, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TerceroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idTercero}")]
    [HttpPut("actualizar/{idTercero}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idTercero, [FromBody] TerceroDto dto, CancellationToken cancellationToken)
    {
        dto.IdTercero = idTercero;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTercero}")]
    [HttpDelete("desactivar/{idTercero}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idTercero, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTercero, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



