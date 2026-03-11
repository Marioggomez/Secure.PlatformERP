using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.modulo.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/modulo")]
public sealed class ModuloController : ControllerBase
{
    private readonly IModuloRepository _repository;

    public ModuloController(IModuloRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ModuloDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idModulo}")]
    public async Task<ActionResult<ModuloDto>> ObtenerAsync([FromRoute] int idModulo, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idModulo, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ModuloDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idModulo = id }, new { id });
    }

    [HttpPut("{idModulo}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idModulo, [FromBody] ModuloDto dto, CancellationToken cancellationToken)
    {
        dto.IdModulo = idModulo;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idModulo}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idModulo, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idModulo, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
