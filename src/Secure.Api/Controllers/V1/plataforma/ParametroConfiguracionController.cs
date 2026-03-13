using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.parametro_configuracion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/parametro_configuracion")]
public sealed class ParametroConfiguracionController : ControllerBase
{
    private readonly IParametroConfiguracionRepository _repository;

    public ParametroConfiguracionController(IParametroConfiguracionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ParametroConfiguracionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idParametroConfiguracion}")]
    [HttpGet("obtener/{idParametroConfiguracion}")]
    public async Task<ActionResult<ParametroConfiguracionDto>> ObtenerAsync([FromRoute] int idParametroConfiguracion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idParametroConfiguracion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ParametroConfiguracionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idParametroConfiguracion = id }, new { id });
    }

    [HttpPut("{idParametroConfiguracion}")]
    [HttpPut("actualizar/{idParametroConfiguracion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idParametroConfiguracion, [FromBody] ParametroConfiguracionDto dto, CancellationToken cancellationToken)
    {
        dto.IdParametroConfiguracion = idParametroConfiguracion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idParametroConfiguracion}")]
    [HttpDelete("desactivar/{idParametroConfiguracion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idParametroConfiguracion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idParametroConfiguracion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


