using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.politica_ip.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/politica_ip")]
public sealed class PoliticaIpController : ControllerBase
{
    private readonly IPoliticaIpRepository _repository;

    public PoliticaIpController(IPoliticaIpRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<PoliticaIpDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idPoliticaIp}")]
    [HttpGet("obtener/{idPoliticaIp}")]
    public async Task<ActionResult<PoliticaIpDto>> ObtenerAsync([FromRoute] long idPoliticaIp, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idPoliticaIp, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] PoliticaIpDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idPoliticaIp}")]
    [HttpPut("actualizar/{idPoliticaIp}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idPoliticaIp, [FromBody] PoliticaIpDto dto, CancellationToken cancellationToken)
    {
        dto.IdPoliticaIp = idPoliticaIp;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idPoliticaIp}")]
    [HttpDelete("desactivar/{idPoliticaIp}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idPoliticaIp, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idPoliticaIp, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



