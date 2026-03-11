using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.desafio_mfa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DesafioMfaRepository : IDesafioMfaRepository
{
    private const string SpListar = "seguridad.usp_desafio_mfa_listar";
    private const string SpObtener = "seguridad.usp_desafio_mfa_obtener";
    private const string SpCrear = "seguridad.usp_desafio_mfa_crear";
    private const string SpActualizar = "seguridad.usp_desafio_mfa_actualizar";
    private const string SpDesactivar = "seguridad.usp_desafio_mfa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public DesafioMfaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<DesafioMfaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<DesafioMfaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new DesafioMfaDto
            {
            IdDesafioMfa = reader.GetGuid(reader.GetOrdinal("id_desafio_mfa")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            IdFlujoAutenticacion = reader.IsDBNull(reader.GetOrdinal("id_flujo_autenticacion")) ? null : reader.GetGuid(reader.GetOrdinal("id_flujo_autenticacion")),
            IdPropositoDesafioMfa = reader.GetInt16(reader.GetOrdinal("id_proposito_desafio_mfa")),
            IdCanalNotificacion = reader.GetInt16(reader.GetOrdinal("id_canal_notificacion")),
            CodigoAccion = reader.IsDBNull(reader.GetOrdinal("codigo_accion")) ? null : reader.GetString(reader.GetOrdinal("codigo_accion")),
            OtpHash = reader.IsDBNull(reader.GetOrdinal("otp_hash")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("otp_hash")),
            OtpSalt = reader.IsDBNull(reader.GetOrdinal("otp_salt")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("otp_salt")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            Intentos = reader.GetInt16(reader.GetOrdinal("intentos")),
            MaxIntentos = reader.GetInt16(reader.GetOrdinal("max_intentos")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ValidadoUtc = reader.IsDBNull(reader.GetOrdinal("validado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("validado_utc"))
            });
        }
        return result;
    }

    public async Task<DesafioMfaDto?> ObtenerAsync(Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new DesafioMfaDto
            {
            IdDesafioMfa = reader.GetGuid(reader.GetOrdinal("id_desafio_mfa")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdSesionUsuario = reader.IsDBNull(reader.GetOrdinal("id_sesion_usuario")) ? null : reader.GetGuid(reader.GetOrdinal("id_sesion_usuario")),
            IdFlujoAutenticacion = reader.IsDBNull(reader.GetOrdinal("id_flujo_autenticacion")) ? null : reader.GetGuid(reader.GetOrdinal("id_flujo_autenticacion")),
            IdPropositoDesafioMfa = reader.GetInt16(reader.GetOrdinal("id_proposito_desafio_mfa")),
            IdCanalNotificacion = reader.GetInt16(reader.GetOrdinal("id_canal_notificacion")),
            CodigoAccion = reader.IsDBNull(reader.GetOrdinal("codigo_accion")) ? null : reader.GetString(reader.GetOrdinal("codigo_accion")),
            OtpHash = reader.IsDBNull(reader.GetOrdinal("otp_hash")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("otp_hash")),
            OtpSalt = reader.IsDBNull(reader.GetOrdinal("otp_salt")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("otp_salt")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            Intentos = reader.GetInt16(reader.GetOrdinal("intentos")),
            MaxIntentos = reader.GetInt16(reader.GetOrdinal("max_intentos")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ValidadoUtc = reader.IsDBNull(reader.GetOrdinal("validado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("validado_utc"))
            };
        }
        return null;
    }

    public async Task<Guid> CrearAsync(DesafioMfaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, dto.IdDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, dto.IdFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_proposito_desafio_mfa", SqlDbType.SmallInt, dto.IdPropositoDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_canal_notificacion", SqlDbType.SmallInt, dto.IdCanalNotificacion));
        command.Parameters.Add(CreateParameter("@codigo_accion", SqlDbType.NVarChar, dto.CodigoAccion, 100));
        command.Parameters.Add(CreateParameter("@otp_hash", SqlDbType.Binary, dto.OtpHash, 32));
        command.Parameters.Add(CreateParameter("@otp_salt", SqlDbType.VarBinary, dto.OtpSalt, 16));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@intentos", SqlDbType.SmallInt, dto.Intentos));
        command.Parameters.Add(CreateParameter("@max_intentos", SqlDbType.SmallInt, dto.MaxIntentos));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@validado_utc", SqlDbType.DateTime2, dto.ValidadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return (result is Guid value ? value : Guid.Parse(result?.ToString() ?? Guid.Empty.ToString()));
    }

    public async Task<bool> ActualizarAsync(DesafioMfaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, dto.IdDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, dto.IdSesionUsuario));
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, dto.IdFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_proposito_desafio_mfa", SqlDbType.SmallInt, dto.IdPropositoDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_canal_notificacion", SqlDbType.SmallInt, dto.IdCanalNotificacion));
        command.Parameters.Add(CreateParameter("@codigo_accion", SqlDbType.NVarChar, dto.CodigoAccion, 100));
        command.Parameters.Add(CreateParameter("@otp_hash", SqlDbType.Binary, dto.OtpHash, 32));
        command.Parameters.Add(CreateParameter("@otp_salt", SqlDbType.VarBinary, dto.OtpSalt, 16));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, dto.ExpiraEnUtc));
        command.Parameters.Add(CreateParameter("@usado", SqlDbType.Bit, dto.Usado));
        command.Parameters.Add(CreateParameter("@intentos", SqlDbType.SmallInt, dto.Intentos));
        command.Parameters.Add(CreateParameter("@max_intentos", SqlDbType.SmallInt, dto.MaxIntentos));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@validado_utc", SqlDbType.DateTime2, dto.ValidadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(Guid idDesafioMfa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));
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
