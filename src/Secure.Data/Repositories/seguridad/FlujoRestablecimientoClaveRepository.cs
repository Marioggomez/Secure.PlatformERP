using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.flujo_restablecimiento_clave con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FlujoRestablecimientoClaveRepository : IFlujoRestablecimientoClaveRepository
{
    private const string SpListar = "seguridad.usp_flujo_restablecimiento_clave_listar";
    private const string SpObtener = "seguridad.usp_flujo_restablecimiento_clave_obtener";
    private const string SpCrear = "seguridad.usp_flujo_restablecimiento_clave_crear";
    private const string SpActualizar = "seguridad.usp_flujo_restablecimiento_clave_actualizar";
    private const string SpDesactivar = "seguridad.usp_flujo_restablecimiento_clave_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public FlujoRestablecimientoClaveRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<FlujoRestablecimientoClaveDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<FlujoRestablecimientoClaveDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new FlujoRestablecimientoClaveDto
            {
            IdFlujoRestablecimientoClave = reader.GetGuid(reader.GetOrdinal("id_flujo_restablecimiento_clave")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoVerificacionRestablecimiento = reader.GetInt16(reader.GetOrdinal("id_tipo_verificacion_restablecimiento")),
            VerificacionCompletada = reader.GetBoolean(reader.GetOrdinal("verificacion_completada")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<FlujoRestablecimientoClaveDto?> ObtenerAsync(Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new FlujoRestablecimientoClaveDto
            {
            IdFlujoRestablecimientoClave = reader.GetGuid(reader.GetOrdinal("id_flujo_restablecimiento_clave")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoVerificacionRestablecimiento = reader.GetInt16(reader.GetOrdinal("id_tipo_verificacion_restablecimiento")),
            VerificacionCompletada = reader.GetBoolean(reader.GetOrdinal("verificacion_completada")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<Guid> CrearAsync(FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, dto.IdFlujoRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_verificacion_restablecimiento", SqlDbType.SmallInt, dto.IdTipoVerificacionRestablecimiento));
        command.Parameters.Add(CreateParameter("@verificacion_completada", SqlDbType.Bit, dto.VerificacionCompletada));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return (result is Guid value ? value : Guid.Parse(result?.ToString() ?? Guid.Empty.ToString()));
    }

    public async Task<bool> ActualizarAsync(FlujoRestablecimientoClaveDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, dto.IdFlujoRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_verificacion_restablecimiento", SqlDbType.SmallInt, dto.IdTipoVerificacionRestablecimiento));
        command.Parameters.Add(CreateParameter("@verificacion_completada", SqlDbType.Bit, dto.VerificacionCompletada));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(Guid idFlujoRestablecimientoClave, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));
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
