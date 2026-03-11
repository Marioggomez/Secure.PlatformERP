using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.auditoria_evento_seguridad con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaEventoSeguridadRepository : IAuditoriaEventoSeguridadRepository
{
    private const string SpListar = "observabilidad.usp_auditoria_evento_seguridad_listar";
    private const string SpObtener = "observabilidad.usp_auditoria_evento_seguridad_obtener";
    private const string SpCrear = "observabilidad.usp_auditoria_evento_seguridad_crear";
    private const string SpActualizar = "observabilidad.usp_auditoria_evento_seguridad_actualizar";
    private const string SpDesactivar = "observabilidad.usp_auditoria_evento_seguridad_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AuditoriaEventoSeguridadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AuditoriaEventoSeguridadDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AuditoriaEventoSeguridadDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AuditoriaEventoSeguridadDto
            {
            IdAuditoriaEventoSeguridad = reader.GetInt64(reader.GetOrdinal("id_auditoria_evento_seguridad")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTipoEventoSeguridad = reader.GetInt16(reader.GetOrdinal("id_tipo_evento_seguridad")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString(reader.GetOrdinal("detalle")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id"))
            });
        }
        return result;
    }

    public async Task<AuditoriaEventoSeguridadDto?> ObtenerAsync(long idAuditoriaEventoSeguridad, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_auditoria_evento_seguridad", SqlDbType.BigInt, idAuditoriaEventoSeguridad));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AuditoriaEventoSeguridadDto
            {
            IdAuditoriaEventoSeguridad = reader.GetInt64(reader.GetOrdinal("id_auditoria_evento_seguridad")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc")),
            IdTipoEventoSeguridad = reader.GetInt16(reader.GetOrdinal("id_tipo_evento_seguridad")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString(reader.GetOrdinal("detalle")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        command.Parameters.Add(CreateParameter("@id_tipo_evento_seguridad", SqlDbType.SmallInt, dto.IdTipoEventoSeguridad));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@detalle", SqlDbType.NVarChar, dto.Detalle, 500));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AuditoriaEventoSeguridadDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_auditoria_evento_seguridad", SqlDbType.BigInt, dto.IdAuditoriaEventoSeguridad));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        command.Parameters.Add(CreateParameter("@id_tipo_evento_seguridad", SqlDbType.SmallInt, dto.IdTipoEventoSeguridad));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@detalle", SqlDbType.NVarChar, dto.Detalle, 500));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idAuditoriaEventoSeguridad, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_auditoria_evento_seguridad", SqlDbType.BigInt, idAuditoriaEventoSeguridad));
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
