using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.configuracion_canal_notificacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ConfiguracionCanalNotificacionRepository : IConfiguracionCanalNotificacionRepository
{
    private const string SpListar = "seguridad.usp_configuracion_canal_notificacion_listar";
    private const string SpObtener = "seguridad.usp_configuracion_canal_notificacion_obtener";
    private const string SpCrear = "seguridad.usp_configuracion_canal_notificacion_crear";
    private const string SpActualizar = "seguridad.usp_configuracion_canal_notificacion_actualizar";
    private const string SpDesactivar = "seguridad.usp_configuracion_canal_notificacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ConfiguracionCanalNotificacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ConfiguracionCanalNotificacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ConfiguracionCanalNotificacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ConfiguracionCanalNotificacionDto
            {
            IdConfiguracionCanalNotificacion = reader.GetInt64(reader.GetOrdinal("id_configuracion_canal_notificacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdCanalNotificacion = reader.GetInt16(reader.GetOrdinal("id_canal_notificacion")),
            Host = reader.IsDBNull(reader.GetOrdinal("host")) ? null : reader.GetString(reader.GetOrdinal("host")),
            Puerto = reader.IsDBNull(reader.GetOrdinal("puerto")) ? null : reader.GetInt32(reader.GetOrdinal("puerto")),
            UsaSsl = reader.GetBoolean(reader.GetOrdinal("usa_ssl")),
            UsuarioTecnico = reader.IsDBNull(reader.GetOrdinal("usuario_tecnico")) ? null : reader.GetString(reader.GetOrdinal("usuario_tecnico")),
            ReferenciaSecreto = reader.IsDBNull(reader.GetOrdinal("referencia_secreto")) ? null : reader.GetString(reader.GetOrdinal("referencia_secreto")),
            SecretoCifrado = reader.IsDBNull(reader.GetOrdinal("secreto_cifrado")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("secreto_cifrado")),
            RemitenteCorreo = reader.IsDBNull(reader.GetOrdinal("remitente_correo")) ? null : reader.GetString(reader.GetOrdinal("remitente_correo")),
            NombreRemitente = reader.IsDBNull(reader.GetOrdinal("nombre_remitente")) ? null : reader.GetString(reader.GetOrdinal("nombre_remitente")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<ConfiguracionCanalNotificacionDto?> ObtenerAsync(long idConfiguracionCanalNotificacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_configuracion_canal_notificacion", SqlDbType.BigInt, idConfiguracionCanalNotificacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ConfiguracionCanalNotificacionDto
            {
            IdConfiguracionCanalNotificacion = reader.GetInt64(reader.GetOrdinal("id_configuracion_canal_notificacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdCanalNotificacion = reader.GetInt16(reader.GetOrdinal("id_canal_notificacion")),
            Host = reader.IsDBNull(reader.GetOrdinal("host")) ? null : reader.GetString(reader.GetOrdinal("host")),
            Puerto = reader.IsDBNull(reader.GetOrdinal("puerto")) ? null : reader.GetInt32(reader.GetOrdinal("puerto")),
            UsaSsl = reader.GetBoolean(reader.GetOrdinal("usa_ssl")),
            UsuarioTecnico = reader.IsDBNull(reader.GetOrdinal("usuario_tecnico")) ? null : reader.GetString(reader.GetOrdinal("usuario_tecnico")),
            ReferenciaSecreto = reader.IsDBNull(reader.GetOrdinal("referencia_secreto")) ? null : reader.GetString(reader.GetOrdinal("referencia_secreto")),
            SecretoCifrado = reader.IsDBNull(reader.GetOrdinal("secreto_cifrado")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("secreto_cifrado")),
            RemitenteCorreo = reader.IsDBNull(reader.GetOrdinal("remitente_correo")) ? null : reader.GetString(reader.GetOrdinal("remitente_correo")),
            NombreRemitente = reader.IsDBNull(reader.GetOrdinal("nombre_remitente")) ? null : reader.GetString(reader.GetOrdinal("nombre_remitente")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_canal_notificacion", SqlDbType.SmallInt, dto.IdCanalNotificacion));
        command.Parameters.Add(CreateParameter("@host", SqlDbType.NVarChar, dto.Host, 200));
        command.Parameters.Add(CreateParameter("@puerto", SqlDbType.Int, dto.Puerto));
        command.Parameters.Add(CreateParameter("@usa_ssl", SqlDbType.Bit, dto.UsaSsl));
        command.Parameters.Add(CreateParameter("@usuario_tecnico", SqlDbType.NVarChar, dto.UsuarioTecnico, 200));
        command.Parameters.Add(CreateParameter("@referencia_secreto", SqlDbType.NVarChar, dto.ReferenciaSecreto, 300));
        command.Parameters.Add(CreateParameter("@secreto_cifrado", SqlDbType.VarBinary, dto.SecretoCifrado, -1));
        command.Parameters.Add(CreateParameter("@remitente_correo", SqlDbType.NVarChar, dto.RemitenteCorreo, 200));
        command.Parameters.Add(CreateParameter("@nombre_remitente", SqlDbType.NVarChar, dto.NombreRemitente, 200));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ConfiguracionCanalNotificacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_configuracion_canal_notificacion", SqlDbType.BigInt, dto.IdConfiguracionCanalNotificacion));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_canal_notificacion", SqlDbType.SmallInt, dto.IdCanalNotificacion));
        command.Parameters.Add(CreateParameter("@host", SqlDbType.NVarChar, dto.Host, 200));
        command.Parameters.Add(CreateParameter("@puerto", SqlDbType.Int, dto.Puerto));
        command.Parameters.Add(CreateParameter("@usa_ssl", SqlDbType.Bit, dto.UsaSsl));
        command.Parameters.Add(CreateParameter("@usuario_tecnico", SqlDbType.NVarChar, dto.UsuarioTecnico, 200));
        command.Parameters.Add(CreateParameter("@referencia_secreto", SqlDbType.NVarChar, dto.ReferenciaSecreto, 300));
        command.Parameters.Add(CreateParameter("@secreto_cifrado", SqlDbType.VarBinary, dto.SecretoCifrado, -1));
        command.Parameters.Add(CreateParameter("@remitente_correo", SqlDbType.NVarChar, dto.RemitenteCorreo, 200));
        command.Parameters.Add(CreateParameter("@nombre_remitente", SqlDbType.NVarChar, dto.NombreRemitente, 200));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idConfiguracionCanalNotificacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_configuracion_canal_notificacion", SqlDbType.BigInt, idConfiguracionCanalNotificacion));
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
