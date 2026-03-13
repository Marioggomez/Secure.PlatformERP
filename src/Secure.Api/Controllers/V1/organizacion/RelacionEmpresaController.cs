using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;

namespace Secure.Platform.Api.Controllers.V1.Organizacion;

/// <summary>
/// Controller API v1 para la tabla organizacion.relacion_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/organizacion/relacion_empresa")]
public sealed class RelacionEmpresaController : ControllerBase
{
    private readonly IRelacionEmpresaRepository _repository;

    public RelacionEmpresaController(IRelacionEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<RelacionEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idRelacionEmpresa}")]
    [HttpGet("obtener/{idRelacionEmpresa}")]
    public async Task<ActionResult<RelacionEmpresaDto>> ObtenerAsync([FromRoute] long idRelacionEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idRelacionEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] RelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idRelacionEmpresa = id }, new { id });
    }

    [HttpPut("{idRelacionEmpresa}")]
    [HttpPut("actualizar/{idRelacionEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idRelacionEmpresa, [FromBody] RelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdRelacionEmpresa = idRelacionEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idRelacionEmpresa}")]
    [HttpDelete("desactivar/{idRelacionEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idRelacionEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idRelacionEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


