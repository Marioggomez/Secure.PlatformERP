using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.feature_flag.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/feature_flag")]
public sealed class FeatureFlagController : ControllerBase
{
    private readonly IFeatureFlagRepository _repository;

    public FeatureFlagController(IFeatureFlagRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FeatureFlagDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idFeature}")]
    public async Task<ActionResult<FeatureFlagDto>> ObtenerAsync([FromRoute] long idFeature, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idFeature, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] FeatureFlagDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idFeature = id }, new { id });
    }

    [HttpPut("{idFeature}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idFeature, [FromBody] FeatureFlagDto dto, CancellationToken cancellationToken)
    {
        dto.IdFeature = idFeature;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idFeature}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idFeature, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idFeature, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
