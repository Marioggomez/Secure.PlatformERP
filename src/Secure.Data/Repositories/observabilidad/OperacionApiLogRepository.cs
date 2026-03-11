using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.operacion_api_log con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class OperacionApiLogRepository : IOperacionApiLogRepository
{
    private const string SpListar = "observabilidad.usp_operacion_api_log_listar";
    private const string SpObtener = "observabilidad.usp_operacion_api_log_obtener";
    private const string SpCrear = "observabilidad.usp_operacion_api_log_crear";
    private const string SpActualizar = "observabilidad.usp_operacion_api_log_actualizar";
    private const string SpDesactivar = "observabilidad.usp_operacion_api_log_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public OperacionApiLogRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<OperacionApiLogDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<OperacionApiLogDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new OperacionApiLogDto
            {
            IdOperacionApiLog = reader.GetInt64(reader.GetOrdinal("id_operacion_api_log")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            CodigoHttp = reader.IsDBNull(reader.GetOrdinal("codigo_http")) ? null : reader.GetInt32(reader.GetOrdinal("codigo_http")),
            DuracionMs = reader.IsDBNull(reader.GetOrdinal("duracion_ms")) ? null : reader.GetInt32(reader.GetOrdinal("duracion_ms")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant"))
            });
        }
        return result;
    }

    public async Task<OperacionApiLogDto?> ObtenerAsync(long idOperacionApiLog, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_operacion_api_log", SqlDbType.BigInt, idOperacionApiLog));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new OperacionApiLogDto
            {
            IdOperacionApiLog = reader.GetInt64(reader.GetOrdinal("id_operacion_api_log")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            CodigoHttp = reader.IsDBNull(reader.GetOrdinal("codigo_http")) ? null : reader.GetInt32(reader.GetOrdinal("codigo_http")),
            DuracionMs = reader.IsDBNull(reader.GetOrdinal("duracion_ms")) ? null : reader.GetInt32(reader.GetOrdinal("duracion_ms")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(OperacionApiLogDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 300));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.VarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@codigo_http", SqlDbType.Int, dto.CodigoHttp));
        command.Parameters.Add(CreateParameter("@duracion_ms", SqlDbType.Int, dto.DuracionMs));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(OperacionApiLogDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_operacion_api_log", SqlDbType.BigInt, dto.IdOperacionApiLog));
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 300));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.VarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@codigo_http", SqlDbType.Int, dto.CodigoHttp));
        command.Parameters.Add(CreateParameter("@duracion_ms", SqlDbType.Int, dto.DuracionMs));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idOperacionApiLog, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_operacion_api_log", SqlDbType.BigInt, idOperacionApiLog));
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
