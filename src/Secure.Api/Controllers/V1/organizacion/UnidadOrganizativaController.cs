using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;

namespace Secure.Platform.Api.Controllers.V1.Organizacion;

/// <summary>
/// Controller API v1 para la tabla organizacion.unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/organizacion/unidad_organizativa")]
public sealed class UnidadOrganizativaController : ControllerBase
{
    private readonly IUnidadOrganizativaRepository _repository;

    public UnidadOrganizativaController(IUnidadOrganizativaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UnidadOrganizativaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUnidadOrganizativa}")]
    [HttpGet("obtener/{idUnidadOrganizativa}")]
    public async Task<ActionResult<UnidadOrganizativaDto>> ObtenerAsync([FromRoute] long idUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUnidadOrganizativa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idUnidadOrganizativa = id }, new { id });
    }

    [HttpPut("{idUnidadOrganizativa}")]
    [HttpPut("actualizar/{idUnidadOrganizativa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUnidadOrganizativa, [FromBody] UnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        dto.IdUnidadOrganizativa = idUnidadOrganizativa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUnidadOrganizativa}")]
    [HttpDelete("desactivar/{idUnidadOrganizativa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUnidadOrganizativa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


