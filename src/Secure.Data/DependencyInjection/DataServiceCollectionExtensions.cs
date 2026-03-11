using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.DependencyInjection;

/// <summary>
/// Registro de servicios de acceso a datos ADO.NET.
/// Autor: Mario Gomez.
/// </summary>
public static class DataServiceCollectionExtensions
{
    /// <summary>
    /// Registra la fabrica de conexion y los repositorios generados.
    /// </summary>
    public static IServiceCollection AddSecureData(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        var assembly = Assembly.GetExecutingAssembly();
        var repositories = assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Repository", StringComparison.Ordinal));

        foreach (var repositoryType in repositories)
        {
            var contract = repositoryType
                .GetInterfaces()
                .FirstOrDefault(item => item.Name.Equals($"I{repositoryType.Name}", StringComparison.Ordinal));

            if (contract is not null)
            {
                services.AddScoped(contract, repositoryType);
            }
        }

        return services;
    }
}
