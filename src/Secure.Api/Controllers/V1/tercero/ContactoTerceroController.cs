using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;

namespace Secure.Platform.Api.Controllers.V1.Tercero;

/// <summary>
/// Controller API v1 para la tabla tercero.contacto_tercero.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/tercero/contacto_tercero")]
public sealed class ContactoTerceroController : ControllerBase
{
    private readonly IContactoTerceroRepository _repository;

    public ContactoTerceroController(IContactoTerceroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<ContactoTerceroDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idContactoTercero}")]
    [HttpGet("obtener/{idContactoTercero}")]
    public async Task<ActionResult<ContactoTerceroDto>> ObtenerAsync([FromRoute] long idContactoTercero, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idContactoTercero, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ContactoTerceroDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idContactoTercero = id }, new { id });
    }

    [HttpPut("{idContactoTercero}")]
    [HttpPut("actualizar/{idContactoTercero}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idContactoTercero, [FromBody] ContactoTerceroDto dto, CancellationToken cancellationToken)
    {
        dto.IdContactoTercero = idContactoTercero;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idContactoTercero}")]
    [HttpDelete("desactivar/{idContactoTercero}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idContactoTercero, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idContactoTercero, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


