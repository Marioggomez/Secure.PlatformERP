using System.Text.Json;

namespace Secure.Platform.Common.Configuration;

/// <summary>
/// Carga centralizada de database.config.json.
/// Autor: Mario Gomez.
/// </summary>
public static class DatabaseConfigLoader
{
    /// <summary>
    /// Carga y valida el archivo database.config.json.
    /// </summary>
    public static DatabaseConfig Load(string absolutePath)
    {
        var json = File.ReadAllText(absolutePath);
        using var document = JsonDocument.Parse(json);

        var root = document.RootElement;
        var provider = root.TryGetProperty("provider", out var providerElement)
            ? providerElement.GetString() ?? string.Empty
            : string.Empty;

        var connectionString = root.TryGetProperty("connectionString", out var connectionStringElement)
            ? connectionStringElement.GetString() ?? string.Empty
            : string.Empty;

        return new DatabaseConfig
        {
            Provider = provider,
            ConnectionString = connectionString
        };
    }
}
