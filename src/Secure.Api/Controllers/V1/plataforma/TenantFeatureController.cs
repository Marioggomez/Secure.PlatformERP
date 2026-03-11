using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.tenant_feature.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/tenant_feature")]
public sealed class TenantFeatureController : ControllerBase
{
    private readonly ITenantFeatureRepository _repository;

    public TenantFeatureController(ITenantFeatureRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TenantFeatureDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTenantFeature}")]
    public async Task<ActionResult<TenantFeatureDto>> ObtenerAsync([FromRoute] long idTenantFeature, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTenantFeature, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TenantFeatureDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTenantFeature = id }, new { id });
    }

    [HttpPut("{idTenantFeature}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idTenantFeature, [FromBody] TenantFeatureDto dto, CancellationToken cancellationToken)
    {
        dto.IdTenantFeature = idTenantFeature;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTenantFeature}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idTenantFeature, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTenantFeature, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
