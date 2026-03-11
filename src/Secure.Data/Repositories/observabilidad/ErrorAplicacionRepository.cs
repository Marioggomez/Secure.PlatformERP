using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.error_aplicacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ErrorAplicacionRepository : IErrorAplicacionRepository
{
    private const string SpListar = "observabilidad.usp_error_aplicacion_listar";
    private const string SpObtener = "observabilidad.usp_error_aplicacion_obtener";
    private const string SpCrear = "observabilidad.usp_error_aplicacion_crear";
    private const string SpActualizar = "observabilidad.usp_error_aplicacion_actualizar";
    private const string SpDesactivar = "observabilidad.usp_error_aplicacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ErrorAplicacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ErrorAplicacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ErrorAplicacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ErrorAplicacionDto
            {
            IdErrorAplicacion = reader.GetInt64(reader.GetOrdinal("id_error_aplicacion")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("endpoint")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            QueryString = reader.IsDBNull(reader.GetOrdinal("query_string")) ? null : reader.GetString(reader.GetOrdinal("query_string")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            TipoError = reader.IsDBNull(reader.GetOrdinal("tipo_error")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_error")),
            MensajeError = reader.IsDBNull(reader.GetOrdinal("mensaje_error")) ? null : reader.GetString(reader.GetOrdinal("mensaje_error")),
            TrazaError = reader.IsDBNull(reader.GetOrdinal("traza_error")) ? null : reader.GetString(reader.GetOrdinal("traza_error")),
            MensajeInterno = reader.IsDBNull(reader.GetOrdinal("mensaje_interno")) ? null : reader.GetString(reader.GetOrdinal("mensaje_interno")),
            TrazaInterna = reader.IsDBNull(reader.GetOrdinal("traza_interna")) ? null : reader.GetString(reader.GetOrdinal("traza_interna")),
            OrigenError = reader.IsDBNull(reader.GetOrdinal("origen_error")) ? null : reader.GetString(reader.GetOrdinal("origen_error")),
            CodigoHttp = reader.IsDBNull(reader.GetOrdinal("codigo_http")) ? null : reader.GetInt32(reader.GetOrdinal("codigo_http"))
            });
        }
        return result;
    }

    public async Task<ErrorAplicacionDto?> ObtenerAsync(long idErrorAplicacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_error_aplicacion", SqlDbType.BigInt, idErrorAplicacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ErrorAplicacionDto
            {
            IdErrorAplicacion = reader.GetInt64(reader.GetOrdinal("id_error_aplicacion")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("endpoint")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            QueryString = reader.IsDBNull(reader.GetOrdinal("query_string")) ? null : reader.GetString(reader.GetOrdinal("query_string")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            TipoError = reader.IsDBNull(reader.GetOrdinal("tipo_error")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_error")),
            MensajeError = reader.IsDBNull(reader.GetOrdinal("mensaje_error")) ? null : reader.GetString(reader.GetOrdinal("mensaje_error")),
            TrazaError = reader.IsDBNull(reader.GetOrdinal("traza_error")) ? null : reader.GetString(reader.GetOrdinal("traza_error")),
            MensajeInterno = reader.IsDBNull(reader.GetOrdinal("mensaje_interno")) ? null : reader.GetString(reader.GetOrdinal("mensaje_interno")),
            TrazaInterna = reader.IsDBNull(reader.GetOrdinal("traza_interna")) ? null : reader.GetString(reader.GetOrdinal("traza_interna")),
            OrigenError = reader.IsDBNull(reader.GetOrdinal("origen_error")) ? null : reader.GetString(reader.GetOrdinal("origen_error")),
            CodigoHttp = reader.IsDBNull(reader.GetOrdinal("codigo_http")) ? null : reader.GetInt32(reader.GetOrdinal("codigo_http"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ErrorAplicacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.NVarChar, dto.Endpoint, 200));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@query_string", SqlDbType.NVarChar, dto.QueryString, 2000));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@tipo_error", SqlDbType.NVarChar, dto.TipoError, 200));
        command.Parameters.Add(CreateParameter("@mensaje_error", SqlDbType.NVarChar, dto.MensajeError, -1));
        command.Parameters.Add(CreateParameter("@traza_error", SqlDbType.NVarChar, dto.TrazaError, -1));
        command.Parameters.Add(CreateParameter("@mensaje_interno", SqlDbType.NVarChar, dto.MensajeInterno, -1));
        command.Parameters.Add(CreateParameter("@traza_interna", SqlDbType.NVarChar, dto.TrazaInterna, -1));
        command.Parameters.Add(CreateParameter("@origen_error", SqlDbType.NVarChar, dto.OrigenError, 200));
        command.Parameters.Add(CreateParameter("@codigo_http", SqlDbType.Int, dto.CodigoHttp));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ErrorAplicacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_error_aplicacion", SqlDbType.BigInt, dto.IdErrorAplicacion));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.NVarChar, dto.Endpoint, 200));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@query_string", SqlDbType.NVarChar, dto.QueryString, 2000));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@tipo_error", SqlDbType.NVarChar, dto.TipoError, 200));
        command.Parameters.Add(CreateParameter("@mensaje_error", SqlDbType.NVarChar, dto.MensajeError, -1));
        command.Parameters.Add(CreateParameter("@traza_error", SqlDbType.NVarChar, dto.TrazaError, -1));
        command.Parameters.Add(CreateParameter("@mensaje_interno", SqlDbType.NVarChar, dto.MensajeInterno, -1));
        command.Parameters.Add(CreateParameter("@traza_interna", SqlDbType.NVarChar, dto.TrazaInterna, -1));
        command.Parameters.Add(CreateParameter("@origen_error", SqlDbType.NVarChar, dto.OrigenError, 200));
        command.Parameters.Add(CreateParameter("@codigo_http", SqlDbType.Int, dto.CodigoHttp));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idErrorAplicacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_error_aplicacion", SqlDbType.BigInt, idErrorAplicacion));
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
