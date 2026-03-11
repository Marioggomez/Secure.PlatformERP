using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Infrastructure.Observability;

/// <summary>
/// Persistencia ADO.NET para auditoria API y errores tecnicos.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ApiObservabilityService : IApiObservabilityService
{
    private readonly IDbConnectionFactory _connectionFactory;

    /// <summary>
    /// Inicializa el servicio de observabilidad.
    /// </summary>
    public ApiObservabilityService(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <inheritdoc />
    public async Task LogOperationAsync(
        Guid? correlationId,
        string? endpoint,
        string? metodoHttp,
        string? usuario,
        int? codigoHttp,
        int? duracionMs,
        string? ip,
        long? idTenant,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "observabilidad.usp_operacion_api_log_crear";

        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, correlationId));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, endpoint, 300));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.VarChar, metodoHttp, 16));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        command.Parameters.Add(CreateParameter("@codigo_http", SqlDbType.Int, codigoHttp));
        command.Parameters.Add(CreateParameter("@duracion_ms", SqlDbType.Int, duracionMs));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, ip, 64));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, DateTime.UtcNow));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task LogErrorAsync(
        Guid? correlationId,
        string? usuario,
        string? endpoint,
        string? mensajeError,
        string? stackTrace,
        string? payload,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "observabilidad.usp_error_log_crear";

        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, correlationId));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, endpoint, 300));
        command.Parameters.Add(CreateParameter("@mensaje_error", SqlDbType.VarChar, mensajeError, -1));
        command.Parameters.Add(CreateParameter("@stacktrace", SqlDbType.VarChar, stackTrace, -1));
        command.Parameters.Add(CreateParameter("@payload", SqlDbType.VarChar, payload, -1));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, DateTime.UtcNow));

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue
            ? new SqlParameter(name, type, size.Value)
            : new SqlParameter(name, type);

        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
