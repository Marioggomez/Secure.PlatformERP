using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_identificador.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_identificador")]
public sealed class UsuarioIdentificadorController : ControllerBase
{
    private readonly IUsuarioIdentificadorRepository _repository;

    public UsuarioIdentificadorController(IUsuarioIdentificadorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioIdentificadorDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuarioIdentificador}")]
    public async Task<ActionResult<UsuarioIdentificadorDto>> ObtenerAsync([FromRoute] long idUsuarioIdentificador, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuarioIdentificador, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioIdentificadorDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idUsuarioIdentificador = id }, new { id });
    }

    [HttpPut("{idUsuarioIdentificador}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuarioIdentificador, [FromBody] UsuarioIdentificadorDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuarioIdentificador = idUsuarioIdentificador;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuarioIdentificador}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuarioIdentificador, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuarioIdentificador, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
