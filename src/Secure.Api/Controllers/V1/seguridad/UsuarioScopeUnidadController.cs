using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_scope_unidad.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_scope_unidad")]
public sealed class UsuarioScopeUnidadController : ControllerBase
{
    private readonly IUsuarioScopeUnidadRepository _repository;

    public UsuarioScopeUnidadController(IUsuarioScopeUnidadRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioScopeUnidadDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuarioScopeUnidad}")]
    public async Task<ActionResult<UsuarioScopeUnidadDto>> ObtenerAsync([FromRoute] long idUsuarioScopeUnidad, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuarioScopeUnidad, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioScopeUnidadDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idUsuarioScopeUnidad = id }, new { id });
    }

    [HttpPut("{idUsuarioScopeUnidad}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuarioScopeUnidad, [FromBody] UsuarioScopeUnidadDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuarioScopeUnidad = idUsuarioScopeUnidad;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuarioScopeUnidad}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuarioScopeUnidad, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuarioScopeUnidad, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
