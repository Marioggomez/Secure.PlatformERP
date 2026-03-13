using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.entidad_alcance_dato.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/entidad_alcance_dato")]
public sealed class EntidadAlcanceDatoController : ControllerBase
{
    private readonly IEntidadAlcanceDatoRepository _repository;

    public EntidadAlcanceDatoController(IEntidadAlcanceDatoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EntidadAlcanceDatoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{codigoEntidad}")]
    [HttpGet("obtener/{codigoEntidad}")]
    public async Task<ActionResult<EntidadAlcanceDatoDto>> ObtenerAsync([FromRoute] string codigoEntidad, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(codigoEntidad, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EntidadAlcanceDatoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{codigoEntidad}")]
    [HttpPut("actualizar/{codigoEntidad}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] string codigoEntidad, [FromBody] EntidadAlcanceDatoDto dto, CancellationToken cancellationToken)
    {
        dto.CodigoEntidad = codigoEntidad;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{codigoEntidad}")]
    [HttpDelete("desactivar/{codigoEntidad}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] string codigoEntidad, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(codigoEntidad, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



