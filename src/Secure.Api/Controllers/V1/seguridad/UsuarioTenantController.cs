using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_tenant.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_tenant")]
public sealed class UsuarioTenantController : ControllerBase
{
    private readonly IUsuarioTenantRepository _repository;

    public UsuarioTenantController(IUsuarioTenantRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UsuarioTenantDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuario}")]
    [HttpGet("obtener/{idUsuario}")]
    public async Task<ActionResult<UsuarioTenantDto>> ObtenerAsync([FromRoute] long idUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioTenantDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idUsuario}")]
    [HttpPut("actualizar/{idUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuario, [FromBody] UsuarioTenantDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuario = idUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuario}")]
    [HttpDelete("desactivar/{idUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



