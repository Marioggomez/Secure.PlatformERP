namespace Secure.Platform.Common.Configuration;

/// <summary>
/// Modelo de configuracion para la conexion a base de datos.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DatabaseConfig
{
    /// <summary>
    /// Proveedor configurado.
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Cadena de conexion SQL Server.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
}
