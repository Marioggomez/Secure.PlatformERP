using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.version_esquema.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/version_esquema")]
public sealed class VersionEsquemaController : ControllerBase
{
    private readonly IVersionEsquemaRepository _repository;

    public VersionEsquemaController(IVersionEsquemaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<VersionEsquemaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idVersionEsquema}")]
    [HttpGet("obtener/{idVersionEsquema}")]
    public async Task<ActionResult<VersionEsquemaDto>> ObtenerAsync([FromRoute] long idVersionEsquema, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idVersionEsquema, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] VersionEsquemaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idVersionEsquema}")]
    [HttpPut("actualizar/{idVersionEsquema}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idVersionEsquema, [FromBody] VersionEsquemaDto dto, CancellationToken cancellationToken)
    {
        dto.IdVersionEsquema = idVersionEsquema;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idVersionEsquema}")]
    [HttpDelete("desactivar/{idVersionEsquema}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idVersionEsquema, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idVersionEsquema, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



