using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.auditoria_autorizacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaAutorizacionRepository : IAuditoriaAutorizacionRepository
{
    private const string SpListar = "observabilidad.usp_auditoria_autorizacion_listar";
    private const string SpObtener = "observabilidad.usp_auditoria_autorizacion_obtener";
    private const string SpCrear = "observabilidad.usp_auditoria_autorizacion_crear";
    private const string SpActualizar = "observabilidad.usp_auditoria_autorizacion_actualizar";
    private const string SpDesactivar = "observabilidad.usp_auditoria_autorizacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AuditoriaAutorizacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AuditoriaAutorizacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AuditoriaAutorizacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AuditoriaAutorizacionDto
            {
            IdAuditoriaAutorizacion = reader.GetInt64(reader.GetOrdinal("id_auditoria_autorizacion")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            CodigoPermiso = reader.IsDBNull(reader.GetOrdinal("codigo_permiso")) ? null : reader.GetString(reader.GetOrdinal("codigo_permiso")),
            CodigoOperacion = reader.IsDBNull(reader.GetOrdinal("codigo_operacion")) ? null : reader.GetString(reader.GetOrdinal("codigo_operacion")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            Permitido = reader.GetBoolean(reader.GetOrdinal("permitido")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            IdObjeto = reader.IsDBNull(reader.GetOrdinal("id_objeto")) ? null : reader.GetInt64(reader.GetOrdinal("id_objeto")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id"))
            });
        }
        return result;
    }

    public async Task<AuditoriaAutorizacionDto?> ObtenerAsync(long idAuditoriaAutorizacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_auditoria_autorizacion", SqlDbType.BigInt, idAuditoriaAutorizacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AuditoriaAutorizacionDto
            {
            IdAuditoriaAutorizacion = reader.GetInt64(reader.GetOrdinal("id_auditoria_autorizacion")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            CodigoPermiso = reader.IsDBNull(reader.GetOrdinal("codigo_permiso")) ? null : reader.GetString(reader.GetOrdinal("codigo_permiso")),
            CodigoOperacion = reader.IsDBNull(reader.GetOrdinal("codigo_operacion")) ? null : reader.GetString(reader.GetOrdinal("codigo_operacion")),
            MetodoHttp = reader.IsDBNull(reader.GetOrdinal("metodo_http")) ? null : reader.GetString(reader.GetOrdinal("metodo_http")),
            Permitido = reader.GetBoolean(reader.GetOrdinal("permitido")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            IdObjeto = reader.IsDBNull(reader.GetOrdinal("id_objeto")) ? null : reader.GetInt64(reader.GetOrdinal("id_objeto")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AuditoriaAutorizacionDto dto, CancellationToken cancellationToken)
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
        command.Parameters.Add(CreateParameter("@codigo_permiso", SqlDbType.NVarChar, dto.CodigoPermiso, 150));
        command.Parameters.Add(CreateParameter("@codigo_operacion", SqlDbType.NVarChar, dto.CodigoOperacion, 150));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@permitido", SqlDbType.Bit, dto.Permitido));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 200));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@id_objeto", SqlDbType.BigInt, dto.IdObjeto));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AuditoriaAutorizacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_auditoria_autorizacion", SqlDbType.BigInt, dto.IdAuditoriaAutorizacion));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@codigo_permiso", SqlDbType.NVarChar, dto.CodigoPermiso, 150));
        command.Parameters.Add(CreateParameter("@codigo_operacion", SqlDbType.NVarChar, dto.CodigoOperacion, 150));
        command.Parameters.Add(CreateParameter("@metodo_http", SqlDbType.NVarChar, dto.MetodoHttp, 10));
        command.Parameters.Add(CreateParameter("@permitido", SqlDbType.Bit, dto.Permitido));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 200));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@id_objeto", SqlDbType.BigInt, dto.IdObjeto));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idAuditoriaAutorizacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_auditoria_autorizacion", SqlDbType.BigInt, idAuditoriaAutorizacion));
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
