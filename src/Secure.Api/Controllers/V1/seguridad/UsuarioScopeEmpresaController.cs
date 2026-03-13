using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_scope_empresa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_scope_empresa")]
public sealed class UsuarioScopeEmpresaController : ControllerBase
{
    private readonly IUsuarioScopeEmpresaRepository _repository;

    public UsuarioScopeEmpresaController(IUsuarioScopeEmpresaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UsuarioScopeEmpresaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuarioScopeEmpresa}")]
    [HttpGet("obtener/{idUsuarioScopeEmpresa}")]
    public async Task<ActionResult<UsuarioScopeEmpresaDto>> ObtenerAsync([FromRoute] long idUsuarioScopeEmpresa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuarioScopeEmpresa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idUsuarioScopeEmpresa}")]
    [HttpPut("actualizar/{idUsuarioScopeEmpresa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuarioScopeEmpresa, [FromBody] UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuarioScopeEmpresa = idUsuarioScopeEmpresa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuarioScopeEmpresa}")]
    [HttpDelete("desactivar/{idUsuarioScopeEmpresa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuarioScopeEmpresa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuarioScopeEmpresa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



