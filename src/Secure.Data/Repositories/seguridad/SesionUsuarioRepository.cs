using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.sesion_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionUsuarioRepository : ISesionUsuarioRepository
{
    private const string SpListar = "seguridad.usp_sesion_usuario_listar";
    private const string SpObtener = "seguridad.usp_sesion_usuario_obtener";
    private const string SpCrear = "seguridad.usp_sesion_usuario_crear";
    private const string SpActualizar = "seguridad.usp_sesion_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_sesion_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public SesionUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<SesionUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<SesionUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new SesionUsuarioDto
            {
            IdSesionUsuario = reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            TokenHash = reader.IsDBNull(reader.GetOrdinal("token_hash")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("token_hash")),
            RefreshHash = reader.IsDBNull(reader.GetOrdinal("refresh_hash")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("refresh_hash")),
            OrigenAutenticacion = reader.IsDBNull(reader.GetOrdinal("origen_autenticacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("origen_autenticacion")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ExpiraAbsolutaUtc = reader.GetDateTime(reader.GetOrdinal("expira_absoluta_utc")),
            UltimaActividadUtc = reader.GetDateTime(reader.GetOrdinal("ultima_actividad_utc")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            HuellaDispositivo = reader.IsDBNull(reader.GetOrdinal("huella_dispositivo")) ? null : reader.GetString(reader.GetOrdinal("huella_dispositivo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            RevocadaUtc = reader.IsDBNull(reader.GetOrdinal("revocada_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("revocada_utc")),
            MotivoRevocacion = reader.IsDBNull(reader.GetOrdinal("motivo_revocacion")) ? null : reader.GetString(reader.GetOrdinal("motivo_revocacion")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<SesionUsuarioDto?> ObtenerAsync(Guid idSesionUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, idSesionUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new SesionUsuarioDto
            {
            IdSesionUsuario = reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            TokenHash = reader.IsDBNull(reader.GetOrdinal("token_hash")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("token_hash")),
            RefreshHash = reader.IsDBNull(reader.GetOrdinal("refresh_hash")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("refresh_hash")),
            OrigenAutenticacion = reader.IsDBNull(reader.GetOrdinal("origen_autenticacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("origen_autenticacion")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ExpiraAbsolutaUtc = reader.GetDateTime(reader.GetOrdinal("expira_absoluta_utc")),
            UltimaActividadUtc = reader.GetDateTime(reader.GetOrdinal("ultima_actividad_utc")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            HuellaDispositivo = reader.IsDBNull(reader.GetOrdinal("huella_dispositivo")) ? null : reader.GetString(reader.GetOrdinal("huella_dispositivo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            RevocadaUtc = reader.IsDBNull(reader.GetOrdinal("revocada_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("revocada_utc")),
            MotivoRevocacion = reader.IsDBNull(reader.GetOrdinal("motivo_revocacion")) ? null : reader.GetString(reader.GetOrdinal("motivo_revocacion")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<Guid> CrearAsync(SesionUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@token_hash", SqlDbType.Binary, dto.TokenHash, 32));
        command.Parameters.Add(CreateParameter("@refresh_hash", SqlDbType.Binary, dto.RefreshHash, 32));
        command.Parameters.Add(CreateParameter("@origen_autenticacion", SqlDbType.VarChar, dto.OrigenAutenticacion, 20));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@expira_absoluta_utc", SqlDbType.DateTime2, dto.ExpiraAbsolutaUtc));
        command.Parameters.Add(CreateParameter("@ultima_actividad_utc", SqlDbType.DateTime2, dto.UltimaActividadUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, dto.HuellaDispositivo, 200));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@revocada_utc", SqlDbType.DateTime2, dto.RevocadaUtc));
        command.Parameters.Add(CreateParameter("@motivo_revocacion", SqlDbType.NVarChar, dto.MotivoRevocacion, 200));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return (result is Guid value ? value : Guid.Parse(result?.ToString() ?? Guid.Empty.ToString()));
    }

    public async Task<bool> ActualizarAsync(SesionUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@token_hash", SqlDbType.Binary, dto.TokenHash, 32));
        command.Parameters.Add(CreateParameter("@refresh_hash", SqlDbType.Binary, dto.RefreshHash, 32));
        command.Parameters.Add(CreateParameter("@origen_autenticacion", SqlDbType.VarChar, dto.OrigenAutenticacion, 20));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@expira_absoluta_utc", SqlDbType.DateTime2, dto.ExpiraAbsolutaUtc));
        command.Parameters.Add(CreateParameter("@ultima_actividad_utc", SqlDbType.DateTime2, dto.UltimaActividadUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, dto.HuellaDispositivo, 200));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@revocada_utc", SqlDbType.DateTime2, dto.RevocadaUtc));
        command.Parameters.Add(CreateParameter("@motivo_revocacion", SqlDbType.NVarChar, dto.MotivoRevocacion, 200));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(Guid idSesionUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, idSesionUsuario));
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
