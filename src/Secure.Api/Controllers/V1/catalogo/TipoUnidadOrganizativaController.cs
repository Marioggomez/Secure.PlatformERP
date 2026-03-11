using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_unidad_organizativa")]
public sealed class TipoUnidadOrganizativaController : ControllerBase
{
    private readonly ITipoUnidadOrganizativaRepository _repository;

    public TipoUnidadOrganizativaController(ITipoUnidadOrganizativaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TipoUnidadOrganizativaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoUnidadOrganizativa}")]
    public async Task<ActionResult<TipoUnidadOrganizativaDto>> ObtenerAsync([FromRoute] short idTipoUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoUnidadOrganizativa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoUnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoUnidadOrganizativa = id }, new { id });
    }

    [HttpPut("{idTipoUnidadOrganizativa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoUnidadOrganizativa, [FromBody] TipoUnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoUnidadOrganizativa = idTipoUnidadOrganizativa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoUnidadOrganizativa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoUnidadOrganizativa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
