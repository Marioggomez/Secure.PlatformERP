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
        var connection = new SqlConnection(_connectionString);
        connection.StateChange += (_, args) =>
        {
            if (args.CurrentState != System.Data.ConnectionState.Open)
            {
                return;
            }

            ApplySessionContext(connection);
        };

        return connection;
    }

    private static void ApplySessionContext(SqlConnection connection)
    {
        var scope = SqlScopeContext.Current;
        if (scope is null)
        {
            return;
        }

        using var command = connection.CreateCommand();
        command.CommandText = @"
EXEC sys.sp_set_session_context @key = N'id_tenant', @value = @id_tenant;
EXEC sys.sp_set_session_context @key = N'id_empresa', @value = @id_empresa;
EXEC sys.sp_set_session_context @key = N'id_usuario', @value = @id_usuario;";

        command.Parameters.Add(new SqlParameter("@id_tenant", System.Data.SqlDbType.BigInt) { Value = scope.IdTenant });
        command.Parameters.Add(new SqlParameter("@id_empresa", System.Data.SqlDbType.BigInt) { Value = scope.IdEmpresa });
        command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.BigInt) { Value = scope.IdUsuario });

        command.ExecuteNonQuery();
    }
}
