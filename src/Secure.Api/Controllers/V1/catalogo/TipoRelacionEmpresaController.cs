using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_relacion_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_relacion_empresa")]
public sealed class TipoRelacionEmpresaController : ControllerBase
{
    private readonly ITipoRelacionEmpresaRepository _repository;

    public TipoRelacionEmpresaController(ITipoRelacionEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TipoRelacionEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoRelacionEmpresa}")]
    public async Task<ActionResult<TipoRelacionEmpresaDto>> ObtenerAsync([FromRoute] short idTipoRelacionEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoRelacionEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoRelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoRelacionEmpresa = id }, new { id });
    }

    [HttpPut("{idTipoRelacionEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoRelacionEmpresa, [FromBody] TipoRelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoRelacionEmpresa = idTipoRelacionEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoRelacionEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoRelacionEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoRelacionEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
