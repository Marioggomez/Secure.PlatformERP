using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;

namespace Secure.Platform.Api.Controllers.V1.Organizacion;

/// <summary>
/// Controller API v1 para la tabla organizacion.empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/organizacion/empresa")]
public sealed class EmpresaController : ControllerBase
{
    private readonly IEmpresaRepository _repository;

    public EmpresaController(IEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("paginado")]
    [HttpGet("listar/paginado")]
    public async Task<ActionResult<PaginacionResultadoDto<EmpresaListadoDto>>> ListarPaginadoAsync([FromQuery] PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _repository.ListarPaginadoAsync(request, cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEmpresa}")]
    [HttpGet("obtener/{idEmpresa}")]
    public async Task<ActionResult<EmpresaDto>> ObtenerAsync([FromRoute] long idEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idEmpresa}")]
    [HttpPut("actualizar/{idEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idEmpresa, [FromBody] EmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdEmpresa = idEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEmpresa}")]
    [HttpDelete("desactivar/{idEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



