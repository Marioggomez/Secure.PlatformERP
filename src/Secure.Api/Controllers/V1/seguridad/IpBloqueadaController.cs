using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.ip_bloqueada.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/ip_bloqueada")]
public sealed class IpBloqueadaController : ControllerBase
{
    private readonly IIpBloqueadaRepository _repository;

    public IpBloqueadaController(IIpBloqueadaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<IpBloqueadaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idIpBloqueada}")]
    [HttpGet("obtener/{idIpBloqueada}")]
    public async Task<ActionResult<IpBloqueadaDto>> ObtenerAsync([FromRoute] long idIpBloqueada, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idIpBloqueada, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] IpBloqueadaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idIpBloqueada = id }, new { id });
    }

    [HttpPut("{idIpBloqueada}")]
    [HttpPut("actualizar/{idIpBloqueada}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idIpBloqueada, [FromBody] IpBloqueadaDto dto, CancellationToken cancellationToken)
    {
        dto.IdIpBloqueada = idIpBloqueada;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idIpBloqueada}")]
    [HttpDelete("desactivar/{idIpBloqueada}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idIpBloqueada, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idIpBloqueada, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


