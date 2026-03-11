using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.politica_operacion_api.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/politica_operacion_api")]
public sealed class PoliticaOperacionApiController : ControllerBase
{
    private readonly IPoliticaOperacionApiRepository _repository;

    public PoliticaOperacionApiController(IPoliticaOperacionApiRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PoliticaOperacionApiDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPoliticaOperacionApi}")]
    public async Task<ActionResult<PoliticaOperacionApiDto>> ObtenerAsync([FromRoute] long idPoliticaOperacionApi, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPoliticaOperacionApi, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PoliticaOperacionApiDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idPoliticaOperacionApi = id }, new { id });
    }

    [HttpPut("{idPoliticaOperacionApi}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPoliticaOperacionApi, [FromBody] PoliticaOperacionApiDto dto, CancellationToken cancellationToken)
    {
        dto.IdPoliticaOperacionApi = idPoliticaOperacionApi;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPoliticaOperacionApi}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPoliticaOperacionApi, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPoliticaOperacionApi, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
