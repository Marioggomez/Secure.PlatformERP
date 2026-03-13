using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.modulo_permiso.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/modulo_permiso")]
public sealed class ModuloPermisoController : ControllerBase
{
    private readonly IModuloPermisoRepository _repository;

    public ModuloPermisoController(IModuloPermisoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ModuloPermisoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idModuloPermiso}")]
    [HttpGet("obtener/{idModuloPermiso}")]
    public async Task<ActionResult<ModuloPermisoDto>> ObtenerAsync([FromRoute] long idModuloPermiso, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idModuloPermiso, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ModuloPermisoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idModuloPermiso}")]
    [HttpPut("actualizar/{idModuloPermiso}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idModuloPermiso, [FromBody] ModuloPermisoDto dto, CancellationToken cancellationToken)
    {
        dto.IdModuloPermiso = idModuloPermiso;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idModuloPermiso}")]
    [HttpDelete("desactivar/{idModuloPermiso}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idModuloPermiso, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idModuloPermiso, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



