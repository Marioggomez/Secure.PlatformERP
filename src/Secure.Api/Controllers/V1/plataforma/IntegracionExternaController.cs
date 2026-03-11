using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.integracion_externa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/integracion_externa")]
public sealed class IntegracionExternaController : ControllerBase
{
    private readonly IIntegracionExternaRepository _repository;

    public IntegracionExternaController(IIntegracionExternaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<IntegracionExternaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idIntegracion}")]
    public async Task<ActionResult<IntegracionExternaDto>> ObtenerAsync([FromRoute] long idIntegracion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idIntegracion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] IntegracionExternaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idIntegracion = id }, new { id });
    }

    [HttpPut("{idIntegracion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idIntegracion, [FromBody] IntegracionExternaDto dto, CancellationToken cancellationToken)
    {
        dto.IdIntegracion = idIntegracion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idIntegracion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idIntegracion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idIntegracion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
