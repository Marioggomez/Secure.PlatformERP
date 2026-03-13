using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.bitacora_instalacion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/bitacora_instalacion")]
public sealed class BitacoraInstalacionController : ControllerBase
{
    private readonly IBitacoraInstalacionRepository _repository;

    public BitacoraInstalacionController(IBitacoraInstalacionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<BitacoraInstalacionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idBitacoraInstalacion}")]
    [HttpGet("obtener/{idBitacoraInstalacion}")]
    public async Task<ActionResult<BitacoraInstalacionDto>> ObtenerAsync([FromRoute] long idBitacoraInstalacion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idBitacoraInstalacion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] BitacoraInstalacionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idBitacoraInstalacion}")]
    [HttpPut("actualizar/{idBitacoraInstalacion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idBitacoraInstalacion, [FromBody] BitacoraInstalacionDto dto, CancellationToken cancellationToken)
    {
        dto.IdBitacoraInstalacion = idBitacoraInstalacion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idBitacoraInstalacion}")]
    [HttpDelete("desactivar/{idBitacoraInstalacion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idBitacoraInstalacion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idBitacoraInstalacion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



