using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.estado_registro.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/estado_registro")]
public sealed class EstadoRegistroController : ControllerBase
{
    private readonly IEstadoRegistroRepository _repository;

    public EstadoRegistroController(IEstadoRegistroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EstadoRegistroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEstado}")]
    [HttpGet("obtener/{idEstado}")]
    public async Task<ActionResult<EstadoRegistroDto>> ObtenerAsync([FromRoute] int idEstado, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEstado, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EstadoRegistroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEstado = id }, new { id });
    }

    [HttpPut("{idEstado}")]
    [HttpPut("actualizar/{idEstado}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idEstado, [FromBody] EstadoRegistroDto dto, CancellationToken cancellationToken)
    {
        dto.IdEstado = idEstado;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEstado}")]
    [HttpDelete("desactivar/{idEstado}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idEstado, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEstado, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


