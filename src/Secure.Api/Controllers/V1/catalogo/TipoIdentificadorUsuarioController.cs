using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;

namespace Secure.Platform.Api.Controllers.V1.Catalogo;

/// <summary>
/// Controller API v1 para la tabla catalogo.tipo_identificador_usuario.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/catalogo/tipo_identificador_usuario")]
public sealed class TipoIdentificadorUsuarioController : ControllerBase
{
    private readonly ITipoIdentificadorUsuarioRepository _repository;

    public TipoIdentificadorUsuarioController(ITipoIdentificadorUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TipoIdentificadorUsuarioDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idTipoIdentificadorUsuario}")]
    public async Task<ActionResult<TipoIdentificadorUsuarioDto>> ObtenerAsync([FromRoute] short idTipoIdentificadorUsuario, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idTipoIdentificadorUsuario, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] TipoIdentificadorUsuarioDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idTipoIdentificadorUsuario = id }, new { id });
    }

    [HttpPut("{idTipoIdentificadorUsuario}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] short idTipoIdentificadorUsuario, [FromBody] TipoIdentificadorUsuarioDto dto, CancellationToken cancellationToken)
    {
        dto.IdTipoIdentificadorUsuario = idTipoIdentificadorUsuario;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idTipoIdentificadorUsuario}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] short idTipoIdentificadorUsuario, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idTipoIdentificadorUsuario, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
