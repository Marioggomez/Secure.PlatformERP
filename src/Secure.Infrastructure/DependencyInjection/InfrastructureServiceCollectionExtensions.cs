using Microsoft.Extensions.DependencyInjection;
using Secure.Platform.Infrastructure.Observability;
using Secure.Platform.Infrastructure.Security;

namespace Secure.Platform.Infrastructure.DependencyInjection;

/// <summary>
/// Registro de servicios de infraestructura.
/// Autor: Mario Gomez.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra servicios de seguridad y observabilidad.
    /// </summary>
    public static IServiceCollection AddSecureInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IApiObservabilityService, ApiObservabilityService>();
        services.AddSingleton<ISessionTokenService, SessionTokenService>();
        services.AddScoped<IAutenticacionIamService, AutenticacionIamService>();
        return services;
    }
}
