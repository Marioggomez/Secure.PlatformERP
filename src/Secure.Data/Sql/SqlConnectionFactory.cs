using System.Data.SqlClient;

namespace Secure.Platform.Data.Sql;

/// <summary>
/// Implementacion de fabrica de conexion para SQL Server.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    /// <summary>
    /// Inicializa la fabrica con la cadena de conexion configurada.
    /// </summary>
    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <inheritdoc />
    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
