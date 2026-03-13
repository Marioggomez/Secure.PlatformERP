using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.estado_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/estado_usuario")]
public sealed class EstadoUsuarioController : ControllerBase
{
    private readonly IEstadoUsuarioRepository _repository;

    public EstadoUsuarioController(IEstadoUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IReadOnlyList<EstadoUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idEstadoUsuario}")]
    [HttpGet("obtener/{idEstadoUsuario}")]
    public async Task<ActionResult<EstadoUsuarioDto>> ObtenerAsync([FromRoute] short idEstadoUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idEstadoUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    [HttpPost("crear")]
    public async Task<ActionResult<object>> CrearAsync([FromBody] EstadoUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return Ok(new { id });
    }

    [HttpPut("{idEstadoUsuario}")]
    [HttpPut("actualizar/{idEstadoUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idEstadoUsuario, [FromBody] EstadoUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdEstadoUsuario = idEstadoUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idEstadoUsuario}")]
    [HttpDelete("desactivar/{idEstadoUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idEstadoUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idEstadoUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}



