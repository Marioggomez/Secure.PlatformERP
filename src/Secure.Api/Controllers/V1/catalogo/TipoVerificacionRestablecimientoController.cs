using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_verificacion_restablecimiento.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_verificacion_restablecimiento")]
public sealed class TipoVerificacionRestablecimientoController : ControllerBase
{
    private readonly ITipoVerificacionRestablecimientoRepository _repository;

    public TipoVerificacionRestablecimientoController(ITipoVerificacionRestablecimientoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TipoVerificacionRestablecimientoDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoVerificacionRestablecimiento}")]
    public async Task<ActionResult<TipoVerificacionRestablecimientoDto>> ObtenerAsync([FromRoute] short idTipoVerificacionRestablecimiento, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoVerificacionRestablecimiento, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoVerificacionRestablecimientoDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoVerificacionRestablecimiento = id }, new { id });
    }

    [HttpPut("{idTipoVerificacionRestablecimiento}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoVerificacionRestablecimiento, [FromBody] TipoVerificacionRestablecimientoDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoVerificacionRestablecimiento = idTipoVerificacionRestablecimiento;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoVerificacionRestablecimiento}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoVerificacionRestablecimiento, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoVerificacionRestablecimiento, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
