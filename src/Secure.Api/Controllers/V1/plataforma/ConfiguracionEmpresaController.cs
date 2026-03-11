using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.configuracion_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/configuracion_empresa")]
public sealed class ConfiguracionEmpresaController : ControllerBase
{
    private readonly IConfiguracionEmpresaRepository _repository;

    public ConfiguracionEmpresaController(IConfiguracionEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ConfiguracionEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idConfiguracionEmpresa}")]
    public async Task<ActionResult<ConfiguracionEmpresaDto>> ObtenerAsync([FromRoute] long idConfiguracionEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idConfiguracionEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ConfiguracionEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idConfiguracionEmpresa = id }, new { id });
    }

    [HttpPut("{idConfiguracionEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idConfiguracionEmpresa, [FromBody] ConfiguracionEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdConfiguracionEmpresa = idConfiguracionEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idConfiguracionEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idConfiguracionEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idConfiguracionEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
