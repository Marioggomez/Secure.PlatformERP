using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.politica_empresa_override.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/politica_empresa_override")]
public sealed class PoliticaEmpresaOverrideController : ControllerBase
{
    private readonly IPoliticaEmpresaOverrideRepository _repository;

    public PoliticaEmpresaOverrideController(IPoliticaEmpresaOverrideRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PoliticaEmpresaOverrideDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEmpresa}")]
    [HttpGet("obtener/{idEmpresa}")]
    public async Task<ActionResult<PoliticaEmpresaOverrideDto>> ObtenerAsync([FromRoute] long idEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idEmpresa}")]
    [HttpPut("actualizar/{idEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idEmpresa, [FromBody] PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken)
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



