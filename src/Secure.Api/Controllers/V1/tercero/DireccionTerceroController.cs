using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.direccion_tercero.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/direccion_tercero")]
public sealed class DireccionTerceroController : ControllerBase
{
    private readonly IDireccionTerceroRepository _repository;

    public DireccionTerceroController(IDireccionTerceroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<DireccionTerceroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idDireccionTercero}")]
    [HttpGet("obtener/{idDireccionTercero}")]
    public async Task<ActionResult<DireccionTerceroDto>> ObtenerAsync([FromRoute] long idDireccionTercero, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idDireccionTercero, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] DireccionTerceroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idDireccionTercero = id }, new { id });
    }

    [HttpPut("{idDireccionTercero}")]
    [HttpPut("actualizar/{idDireccionTercero}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idDireccionTercero, [FromBody] DireccionTerceroDto dto, CancellationToken cancellationToken)
    {
        dto.IdDireccionTercero = idDireccionTercero;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idDireccionTercero}")]
    [HttpDelete("desactivar/{idDireccionTercero}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idDireccionTercero, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idDireccionTercero, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


