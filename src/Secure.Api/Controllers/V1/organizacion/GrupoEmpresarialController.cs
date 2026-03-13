using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;

namespace Secure.Platform.Api.Controllers.V1.Organizacion;

/// <summary>
/// Controller API v1 para la tabla organizacion.grupo_empresarial.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/organizacion/grupo_empresarial")]
public sealed class GrupoEmpresarialController : ControllerBase
{
    private readonly IGrupoEmpresarialRepository _repository;

    public GrupoEmpresarialController(IGrupoEmpresarialRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<GrupoEmpresarialDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idGrupoEmpresarial}")]
    [HttpGet("obtener/{idGrupoEmpresarial}")]
    public async Task<ActionResult<GrupoEmpresarialDto>> ObtenerAsync([FromRoute] long idGrupoEmpresarial, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idGrupoEmpresarial, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] GrupoEmpresarialDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idGrupoEmpresarial}")]
    [HttpPut("actualizar/{idGrupoEmpresarial}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idGrupoEmpresarial, [FromBody] GrupoEmpresarialDto dto, CancellationToken cancellationToken)
    {
        dto.IdGrupoEmpresarial = idGrupoEmpresarial;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idGrupoEmpresarial}")]
    [HttpDelete("desactivar/{idGrupoEmpresarial}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idGrupoEmpresarial, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idGrupoEmpresarial, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



