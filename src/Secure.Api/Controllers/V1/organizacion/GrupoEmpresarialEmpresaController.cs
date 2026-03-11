using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;

namespace Secure.Platform.Api.Controllers.V1.Organizacion;

/// <summary>
/// Controller API v1 para la tabla organizacion.grupo_empresarial_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/organizacion/grupo_empresarial_empresa")]
public sealed class GrupoEmpresarialEmpresaController : ControllerBase
{
    private readonly IGrupoEmpresarialEmpresaRepository _repository;

    public GrupoEmpresarialEmpresaController(IGrupoEmpresarialEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GrupoEmpresarialEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idGrupoEmpresarial}")]
    public async Task<ActionResult<GrupoEmpresarialEmpresaDto>> ObtenerAsync([FromRoute] long idGrupoEmpresarial, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idGrupoEmpresarial, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idGrupoEmpresarial = id }, new { id });
    }

    [HttpPut("{idGrupoEmpresarial}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idGrupoEmpresarial, [FromBody] GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdGrupoEmpresarial = idGrupoEmpresarial;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idGrupoEmpresarial}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idGrupoEmpresarial, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idGrupoEmpresarial, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
