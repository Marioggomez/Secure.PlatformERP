using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.error_log con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ErrorLogRepository : IErrorLogRepository
{
    private const string SpListar = "observabilidad.usp_error_log_listar";
    private const string SpObtener = "observabilidad.usp_error_log_obtener";
    private const string SpCrear = "observabilidad.usp_error_log_crear";
    private const string SpActualizar = "observabilidad.usp_error_log_actualizar";
    private const string SpDesactivar = "observabilidad.usp_error_log_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ErrorLogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ErrorLogDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ErrorLogDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ErrorLogDto
            {
            IdErrorLog = reader.GetInt64(reader.GetOrdinal("id_error_log")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            MensajeError = reader.IsDBNull(reader.GetOrdinal("mensaje_error")) ? null : reader.GetString(reader.GetOrdinal("mensaje_error")),
            Stacktrace = reader.IsDBNull(reader.GetOrdinal("stacktrace")) ? null : reader.GetString(reader.GetOrdinal("stacktrace")),
            Payload = reader.IsDBNull(reader.GetOrdinal("payload")) ? null : reader.GetString(reader.GetOrdinal("payload")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            });
        }
        return result;
    }

    public async Task<ErrorLogDto?> ObtenerAsync(long idErrorLog, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_error_log", SqlDbType.BigInt, idErrorLog));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ErrorLogDto
            {
            IdErrorLog = reader.GetInt64(reader.GetOrdinal("id_error_log")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            MensajeError = reader.IsDBNull(reader.GetOrdinal("mensaje_error")) ? null : reader.GetString(reader.GetOrdinal("mensaje_error")),
            Stacktrace = reader.IsDBNull(reader.GetOrdinal("stacktrace")) ? null : reader.GetString(reader.GetOrdinal("stacktrace")),
            Payload = reader.IsDBNull(reader.GetOrdinal("payload")) ? null : reader.GetString(reader.GetOrdinal("payload")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ErrorLogDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 300));
        command.Parameters.Add(CreateParameter("@mensaje_error", SqlDbType.VarChar, dto.MensajeError, -1));
        command.Parameters.Add(CreateParameter("@stacktrace", SqlDbType.VarChar, dto.Stacktrace, -1));
        command.Parameters.Add(CreateParameter("@payload", SqlDbType.VarChar, dto.Payload, -1));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ErrorLogDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_error_log", SqlDbType.BigInt, dto.IdErrorLog));
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 300));
        command.Parameters.Add(CreateParameter("@mensaje_error", SqlDbType.VarChar, dto.MensajeError, -1));
        command.Parameters.Add(CreateParameter("@stacktrace", SqlDbType.VarChar, dto.Stacktrace, -1));
        command.Parameters.Add(CreateParameter("@payload", SqlDbType.VarChar, dto.Payload, -1));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idErrorLog, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_error_log", SqlDbType.BigInt, idErrorLog));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
