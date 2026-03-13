namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Servicio para resolver el contexto de sesion a partir del bearer token opaco.
/// Autor: Mario Gomez.
/// </summary>
public interface ISesionContextoService
{
    Task<SesionContextoActual?> ResolverSesionAsync(string tokenPlano, bool actualizarActividadUtc, CancellationToken cancellationToken);
}
