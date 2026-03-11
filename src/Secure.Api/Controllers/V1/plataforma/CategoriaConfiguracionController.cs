using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;

namespace Secure.Platform.Api.Controllers.V1.Plataforma;

/// <summary>
/// Controller API v1 para la tabla plataforma.categoria_configuracion.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/plataforma/categoria_configuracion")]
public sealed class CategoriaConfiguracionController : ControllerBase
{
    private readonly ICategoriaConfiguracionRepository _repository;

    public CategoriaConfiguracionController(ICategoriaConfiguracionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoriaConfiguracionDto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{idCategoriaConfiguracion}")]
    public async Task<ActionResult<CategoriaConfiguracionDto>> ObtenerAsync([FromRoute] int idCategoriaConfiguracion, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync(idCategoriaConfiguracion, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] CategoriaConfiguracionDto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { idCategoriaConfiguracion = id }, new { id });
    }

    [HttpPut("{idCategoriaConfiguracion}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] int idCategoriaConfiguracion, [FromBody] CategoriaConfiguracionDto dto, CancellationToken cancellationToken)
    {
        dto.IdCategoriaConfiguracion = idCategoriaConfiguracion;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{idCategoriaConfiguracion}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] int idCategoriaConfiguracion, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync(idCategoriaConfiguracion, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
