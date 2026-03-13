using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_empresa")]
public sealed class TipoEmpresaController : ControllerBase
{
    private readonly ITipoEmpresaRepository _repository;

    public TipoEmpresaController(ITipoEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoEmpresa}")]
    [HttpGet("obtener/{idTipoEmpresa}")]
    public async Task<ActionResult<TipoEmpresaDto>> ObtenerAsync([FromRoute] short idTipoEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idTipoEmpresa}")]
    [HttpPut("actualizar/{idTipoEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoEmpresa, [FromBody] TipoEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoEmpresa = idTipoEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoEmpresa}")]
    [HttpDelete("desactivar/{idTipoEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



