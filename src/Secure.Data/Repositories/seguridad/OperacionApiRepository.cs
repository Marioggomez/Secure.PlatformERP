using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.operacion_api con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class OperacionApiRepository : IOperacionApiRepository
{
    private const string SpListar = "seguridad.usp_operacion_api_listar";
    private const string SpObtener = "seguridad.usp_operacion_api_obtener";
    private const string SpCrear = "seguridad.usp_operacion_api_crear";
    private const string SpActualizar = "seguridad.usp_operacion_api_actualizar";
    private const string SpDesactivar = "seguridad.usp_operacion_api_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public OperacionApiRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<OperacionApiDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<OperacionApiDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new OperacionApiDto
            {
            IdOperacionApi = reader.GetInt64(reader.GetOrdinal("id_operacion_api")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Modulo = reader.IsDBNull(reader.GetOrdinal("modulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modulo")),
            Controlador = reader.IsDBNull(reader.GetOrdinal("controlador")) ? null : reader.GetString(reader.GetOrdinal("controlador")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? string.Empty : reader.GetString(reader.GetOrdinal("metodo_http")),
            Ruta = reader.IsDBNull(reader.GetOrdinal("ruta")) ? string.Empty : reader.GetString(reader.GetOrdinal("ruta")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<OperacionApiDto?> ObtenerAsync(long idOperacionApi, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_operacion_api", SqlDbType.BigInt, idOperacionApi));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new OperacionApiDto
            {
            IdOperacionApi = reader.GetInt64(reader.GetOrdinal("id_operacion_api")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Modulo = reader.IsDBNull(reader.GetOrdinal("modulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modulo")),
            Controlador = reader.IsDBNull(reader.GetOrdinal("controlador")) ? null : reader.GetString(reader.GetOrdinal("controlador")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? string.Empty : reader.GetString(reader.GetOrdinal("metodo_http")),
            Ruta = reader.IsDBNull(reader.GetOrdinal("ruta")) ? string.Empty : reader.GetString(reader.GetOrdinal("ruta")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(OperacionApiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 150));
        command.Parameters.Add(CreateParameter("@modulo", SqlDbType.NVarChar, dto.Modulo, 80));
        command.Parameters.Add(CreateParameter("@controlador", SqlDbType.NVarChar, dto.Controlador, 120));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 120));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@ruta", SqlDbType.NVarChar, dto.Ruta, 300));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 300));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(OperacionApiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_operacion_api", SqlDbType.BigInt, dto.IdOperacionApi));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 150));
        command.Parameters.Add(CreateParameter("@modulo", SqlDbType.NVarChar, dto.Modulo, 80));
        command.Parameters.Add(CreateParameter("@controlador", SqlDbType.NVarChar, dto.Controlador, 120));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 120));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@ruta", SqlDbType.NVarChar, dto.Ruta, 300));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 300));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idOperacionApi, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_operacion_api", SqlDbType.BigInt, idOperacionApi));
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
