using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.estado_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/estado_empresa")]
public sealed class EstadoEmpresaController : ControllerBase
{
    private readonly IEstadoEmpresaRepository _repository;

    public EstadoEmpresaController(IEstadoEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EstadoEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEstadoEmpresa}")]
    [HttpGet("obtener/{idEstadoEmpresa}")]
    public async Task<ActionResult<EstadoEmpresaDto>> ObtenerAsync([FromRoute] short idEstadoEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEstadoEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EstadoEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEstadoEmpresa = id }, new { id });
    }

    [HttpPut("{idEstadoEmpresa}")]
    [HttpPut("actualizar/{idEstadoEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idEstadoEmpresa, [FromBody] EstadoEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdEstadoEmpresa = idEstadoEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEstadoEmpresa}")]
    [HttpDelete("desactivar/{idEstadoEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idEstadoEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEstadoEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


