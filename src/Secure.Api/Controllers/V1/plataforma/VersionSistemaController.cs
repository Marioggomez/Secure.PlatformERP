using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.version_sistema.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/version_sistema")]
public sealed class VersionSistemaController : ControllerBase
{
    private readonly IVersionSistemaRepository _repository;

    public VersionSistemaController(IVersionSistemaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<VersionSistemaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idVersionSistema}")]
    [HttpGet("obtener/{idVersionSistema}")]
    public async Task<ActionResult<VersionSistemaDto>> ObtenerAsync([FromRoute] int idVersionSistema, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idVersionSistema, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] VersionSistemaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idVersionSistema = id }, new { id });
    }

    [HttpPut("{idVersionSistema}")]
    [HttpPut("actualizar/{idVersionSistema}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idVersionSistema, [FromBody] VersionSistemaDto dto, CancellationToken cancellationToken)
    {
        dto.IdVersionSistema = idVersionSistema;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idVersionSistema}")]
    [HttpDelete("desactivar/{idVersionSistema}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idVersionSistema, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idVersionSistema, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


