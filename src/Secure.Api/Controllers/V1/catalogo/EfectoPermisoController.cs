using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.efecto_permiso.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/efecto_permiso")]
public sealed class EfectoPermisoController : ControllerBase
{
    private readonly IEfectoPermisoRepository _repository;

    public EfectoPermisoController(IEfectoPermisoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EfectoPermisoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEfectoPermiso}")]
    [HttpGet("obtener/{idEfectoPermiso}")]
    public async Task<ActionResult<EfectoPermisoDto>> ObtenerAsync([FromRoute] short idEfectoPermiso, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEfectoPermiso, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EfectoPermisoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idEfectoPermiso = id }, new { id });
    }

    [HttpPut("{idEfectoPermiso}")]
    [HttpPut("actualizar/{idEfectoPermiso}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idEfectoPermiso, [FromBody] EfectoPermisoDto dto, CancellationToken cancellationToken)
    {
        dto.IdEfectoPermiso = idEfectoPermiso;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEfectoPermiso}")]
    [HttpDelete("desactivar/{idEfectoPermiso}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idEfectoPermiso, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEfectoPermiso, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


