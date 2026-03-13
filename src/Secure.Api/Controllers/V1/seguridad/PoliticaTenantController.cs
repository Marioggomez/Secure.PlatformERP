using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.politica_tenant.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/politica_tenant")]
public sealed class PoliticaTenantController : ControllerBase
{
    private readonly IPoliticaTenantRepository _repository;

    public PoliticaTenantController(IPoliticaTenantRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PoliticaTenantDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTenant}")]
    [HttpGet("obtener/{idTenant}")]
    public async Task<ActionResult<PoliticaTenantDto>> ObtenerAsync([FromRoute] long idTenant, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTenant, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PoliticaTenantDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idTenant}")]
    [HttpPut("actualizar/{idTenant}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idTenant, [FromBody] PoliticaTenantDto dto, CancellationToken cancellationToken)
    {
        dto.IdTenant = idTenant;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTenant}")]
    [HttpDelete("desactivar/{idTenant}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idTenant, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTenant, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



