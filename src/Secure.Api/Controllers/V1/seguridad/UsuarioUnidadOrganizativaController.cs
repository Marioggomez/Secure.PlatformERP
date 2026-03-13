using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.usuario_unidad_organizativa.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/usuario_unidad_organizativa")]
public sealed class UsuarioUnidadOrganizativaController : ControllerBase
{
    private readonly IUsuarioUnidadOrganizativaRepository _repository;

    public UsuarioUnidadOrganizativaController(IUsuarioUnidadOrganizativaRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<UsuarioUnidadOrganizativaDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idUsuarioUnidadOrganizativa}")]
    [HttpGet("obtener/{idUsuarioUnidadOrganizativa}")]
    public async Task<ActionResult<UsuarioUnidadOrganizativaDto>> ObtenerAsync([FromRoute] long idUsuarioUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idUsuarioUnidadOrganizativa, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] UsuarioUnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idUsuarioUnidadOrganizativa = id }, new { id });
    }

    [HttpPut("{idUsuarioUnidadOrganizativa}")]
    [HttpPut("actualizar/{idUsuarioUnidadOrganizativa}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idUsuarioUnidadOrganizativa, [FromBody] UsuarioUnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        dto.IdUsuarioUnidadOrganizativa = idUsuarioUnidadOrganizativa;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idUsuarioUnidadOrganizativa}")]
    [HttpDelete("desactivar/{idUsuarioUnidadOrganizativa}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idUsuarioUnidadOrganizativa, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idUsuarioUnidadOrganizativa, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}


