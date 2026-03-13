using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_empresa")]
public sealed class UsuarioEmpresaController : ControllerBase
{
    private readonly IUsuarioEmpresaRepository _repository;

    public UsuarioEmpresaController(IUsuarioEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UsuarioEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuarioEmpresa}")]
    [HttpGet("obtener/{idUsuarioEmpresa}")]
    public async Task<ActionResult<UsuarioEmpresaDto>> ObtenerAsync([FromRoute] long idUsuarioEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuarioEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idUsuarioEmpresa}")]
    [HttpPut("actualizar/{idUsuarioEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuarioEmpresa, [FromBody] UsuarioEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuarioEmpresa = idUsuarioEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuarioEmpresa}")]
    [HttpDelete("desactivar/{idUsuarioEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuarioEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuarioEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



