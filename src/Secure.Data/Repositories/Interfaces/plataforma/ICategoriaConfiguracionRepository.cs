using Secure.Platform.Contracts.Dtos.Plataforma;

namespace Secure.Platform.Data.Repositories.Interfaces.Plataforma;

/// <summary>
/// Contrato del repositorio para plataforma.categoria_configuracion.
/// Autor: Mario Gomez.
/// </summary>
public interface ICategoriaConfiguracionRepository
{
    Task<IReadOnlyList<CategoriaConfiguracionDto>> ListarAsync(CancellationToken cancellationToken);
    Task<CategoriaConfiguracionDto?> ObtenerAsync(int idCategoriaConfiguracion, CancellationToken cancellationToken);
    Task<int> CrearAsync(CategoriaConfiguracionDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(CategoriaConfiguracionDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(int idCategoriaConfiguracion, string? usuario, CancellationToken cancellationToken);
}
