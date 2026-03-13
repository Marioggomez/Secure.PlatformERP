using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.configuracion_canal_notificacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/configuracion_canal_notificacion")]
public sealed class ConfiguracionCanalNotificacionController : ControllerBase
{
    private readonly IConfiguracionCanalNotificacionRepository _repository;

    public ConfiguracionCanalNotificacionController(IConfiguracionCanalNotificacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ConfiguracionCanalNotificacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idConfiguracionCanalNotificacion}")]
    [HttpGet("obtener/{idConfiguracionCanalNotificacion}")]
    public async Task<ActionResult<ConfiguracionCanalNotificacionDto>> ObtenerAsync([FromRoute] long idConfiguracionCanalNotificacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idConfiguracionCanalNotificacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idConfiguracionCanalNotificacion = id }, new { id });
    }

    [HttpPut("{idConfiguracionCanalNotificacion}")]
    [HttpPut("actualizar/{idConfiguracionCanalNotificacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idConfiguracionCanalNotificacion, [FromBody] ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdConfiguracionCanalNotificacion = idConfiguracionCanalNotificacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idConfiguracionCanalNotificacion}")]
    [HttpDelete("desactivar/{idConfiguracionCanalNotificacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idConfiguracionCanalNotificacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idConfiguracionCanalNotificacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


