using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Dbo;
using Secure.Platform.Data.Repositories.Interfaces.Dbo;

namespace Secure.Platform.Api.Controllers.V1.Dbo;

/// <summary>
/// Controller API v1 para la tabla dbo.sysdiagrams.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/dbo/sysdiagrams")]
public sealed class SysdiagramsController : ControllerBase
{
    private readonly ISysdiagramsRepository _repository;

    public SysdiagramsController(ISysdiagramsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<SysdiagramsDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{name}")]
    [HttpGet("obtener/{name}")]
    public async Task<ActionResult<SysdiagramsDto>> ObtenerAsync([FromRoute] string name, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(name, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] SysdiagramsDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { name = id }, new { id });
    }

    [HttpPut("{name}")]
    [HttpPut("actualizar/{name}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] string name, [FromBody] SysdiagramsDto dto, CancellationToken cancellationToken)
    {
        dto.Name = name;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{name}")]
    [HttpDelete("desactivar/{name}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] string name, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(name, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


