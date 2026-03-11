using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.flujo_autenticacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FlujoAutenticacionRepository : IFlujoAutenticacionRepository
{
    private const string SpListar = "seguridad.usp_flujo_autenticacion_listar";
    private const string SpObtener = "seguridad.usp_flujo_autenticacion_obtener";
    private const string SpCrear = "seguridad.usp_flujo_autenticacion_crear";
    private const string SpActualizar = "seguridad.usp_flujo_autenticacion_actualizar";
    private const string SpDesactivar = "seguridad.usp_flujo_autenticacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public FlujoAutenticacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<FlujoAutenticacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<FlujoAutenticacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new FlujoAutenticacionDto
            {
            IdFlujoAutenticacion = reader.GetGuid(reader.GetOrdinal("id_flujo_autenticacion")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            MfaRequerido = reader.GetBoolean(reader.GetOrdinal("mfa_requerido")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            HuellaDispositivo = reader.IsDBNull(reader.GetOrdinal("huella_dispositivo")) ? null : reader.GetString(reader.GetOrdinal("huella_dispositivo")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<FlujoAutenticacionDto?> ObtenerAsync(Guid idFlujoAutenticacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, idFlujoAutenticacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new FlujoAutenticacionDto
            {
            IdFlujoAutenticacion = reader.GetGuid(reader.GetOrdinal("id_flujo_autenticacion")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            MfaRequerido = reader.GetBoolean(reader.GetOrdinal("mfa_requerido")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            HuellaDispositivo = reader.IsDBNull(reader.GetOrdinal("huella_dispositivo")) ? null : reader.GetString(reader.GetOrdinal("huella_dispositivo")),
            SolicitudId = reader.IsDBNull(reader.GetOrdinal("solicitud_id")) ? null : reader.GetString(reader.GetOrdinal("solicitud_id")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<Guid> CrearAsync(FlujoAutenticacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, dto.IdFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@mfa_requerido", SqlDbType.Bit, dto.MfaRequerido));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, dto.HuellaDispositivo, 200));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return (result is Guid value ? value : Guid.Parse(result?.ToString() ?? Guid.Empty.ToString()));
    }

    public async Task<bool> ActualizarAsync(FlujoAutenticacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, dto.IdFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@mfa_requerido", SqlDbType.Bit, dto.MfaRequerido));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, dto.HuellaDispositivo, 200));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, dto.SolicitudId, 64));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(Guid idFlujoAutenticacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, idFlujoAutenticacion));
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
