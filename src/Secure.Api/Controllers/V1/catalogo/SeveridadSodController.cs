using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.severidad_sod.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/severidad_sod")]
public sealed class SeveridadSodController : ControllerBase
{
    private readonly ISeveridadSodRepository _repository;

    public SeveridadSodController(ISeveridadSodRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<SeveridadSodDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idSeveridadSod}")]
    [HttpGet("obtener/{idSeveridadSod}")]
    public async Task<ActionResult<SeveridadSodDto>> ObtenerAsync([FromRoute] short idSeveridadSod, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idSeveridadSod, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] SeveridadSodDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idSeveridadSod}")]
    [HttpPut("actualizar/{idSeveridadSod}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idSeveridadSod, [FromBody] SeveridadSodDto dto, CancellationToken cancellationToken)
    {
        dto.IdSeveridadSod = idSeveridadSod;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idSeveridadSod}")]
    [HttpDelete("desactivar/{idSeveridadSod}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idSeveridadSod, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idSeveridadSod, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



