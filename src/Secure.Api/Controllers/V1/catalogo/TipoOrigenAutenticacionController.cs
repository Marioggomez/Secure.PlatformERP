using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_origen_autenticacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_origen_autenticacion")]
public sealed class TipoOrigenAutenticacionController : ControllerBase
{
    private readonly ITipoOrigenAutenticacionRepository _repository;

    public TipoOrigenAutenticacionController(ITipoOrigenAutenticacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<TipoOrigenAutenticacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoOrigenAutenticacion}")]
    [HttpGet("obtener/{idTipoOrigenAutenticacion}")]
    public async Task<ActionResult<TipoOrigenAutenticacionDto>> ObtenerAsync([FromRoute] short idTipoOrigenAutenticacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoOrigenAutenticacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoOrigenAutenticacion = id }, new { id });
    }

    [HttpPut("{idTipoOrigenAutenticacion}")]
    [HttpPut("actualizar/{idTipoOrigenAutenticacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoOrigenAutenticacion, [FromBody] TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoOrigenAutenticacion = idTipoOrigenAutenticacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoOrigenAutenticacion}")]
    [HttpDelete("desactivar/{idTipoOrigenAutenticacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoOrigenAutenticacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoOrigenAutenticacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


