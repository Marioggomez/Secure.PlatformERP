using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;

namespace Secure.Platform.Api.Controllers.V1.Seguridad;

/// <summary>
/// Controller API v1 para la tabla seguridad.filtro_dato_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/seguridad/filtro_dato_usuario")]
public sealed class FiltroDatoUsuarioController : ControllerBase
{
    private readonly IFiltroDatoUsuarioRepository _repository;

    public FiltroDatoUsuarioController(IFiltroDatoUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<FiltroDatoUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idFiltroDatoUsuario}")]
    [HttpGet("obtener/{idFiltroDatoUsuario}")]
    public async Task<ActionResult<FiltroDatoUsuarioDto>> ObtenerAsync([FromRoute] long idFiltroDatoUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idFiltroDatoUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] FiltroDatoUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idFiltroDatoUsuario}")]
    [HttpPut("actualizar/{idFiltroDatoUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] long idFiltroDatoUsuario, [FromBody] FiltroDatoUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdFiltroDatoUsuario = idFiltroDatoUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idFiltroDatoUsuario}")]
    [HttpDelete("desactivar/{idFiltroDatoUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] long idFiltroDatoUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idFiltroDatoUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



