using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.cuenta_bancaria_tercero.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/cuenta_bancaria_tercero")]
public sealed class CuentaBancariaTerceroController : ControllerBase
{
    private readonly ICuentaBancariaTerceroRepository _repository;

    public CuentaBancariaTerceroController(ICuentaBancariaTerceroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CuentaBancariaTerceroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idCuentaBancariaTercero}")]
    public async Task<ActionResult<CuentaBancariaTerceroDto>> ObtenerAsync([FromRoute] long idCuentaBancariaTercero, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idCuentaBancariaTercero, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] CuentaBancariaTerceroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idCuentaBancariaTercero = id }, new { id });
    }

    [HttpPut("{idCuentaBancariaTercero}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idCuentaBancariaTercero, [FromBody] CuentaBancariaTerceroDto dto, CancellationToken cancellationToken)
    {
        dto.IdCuentaBancariaTercero = idCuentaBancariaTercero;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idCuentaBancariaTercero}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idCuentaBancariaTercero, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idCuentaBancariaTercero, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
