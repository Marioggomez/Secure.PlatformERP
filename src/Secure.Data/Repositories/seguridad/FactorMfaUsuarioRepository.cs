using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.factor_mfa_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class FactorMfaUsuarioRepository : IFactorMfaUsuarioRepository
{
    private const string SpListar = "seguridad.usp_factor_mfa_usuario_listar";
    private const string SpObtener = "seguridad.usp_factor_mfa_usuario_obtener";
    private const string SpCrear = "seguridad.usp_factor_mfa_usuario_crear";
    private const string SpActualizar = "seguridad.usp_factor_mfa_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_factor_mfa_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public FactorMfaUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<FactorMfaUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<FactorMfaUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new FactorMfaUsuarioDto
            {
            IdFactorMfaUsuario = reader.GetInt64(reader.GetOrdinal("id_factor_mfa_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoFactorMfa = reader.GetInt16(reader.GetOrdinal("id_tipo_factor_mfa")),
            Etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? null : reader.GetString(reader.GetOrdinal("etiqueta")),
            DestinoEnmascarado = reader.IsDBNull(reader.GetOrdinal("destino_enmascarado")) ? null : reader.GetString(reader.GetOrdinal("destino_enmascarado")),
            ReferenciaSecreto = reader.IsDBNull(reader.GetOrdinal("referencia_secreto")) ? null : reader.GetString(reader.GetOrdinal("referencia_secreto")),
            SecretoCifrado = reader.IsDBNull(reader.GetOrdinal("secreto_cifrado")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("secreto_cifrado")),
            ConfiguracionJson = reader.IsDBNull(reader.GetOrdinal("configuracion_json")) ? null : reader.GetString(reader.GetOrdinal("configuracion_json")),
            Verificado = reader.GetBoolean(reader.GetOrdinal("verificado")),
            EsPredeterminado = reader.GetBoolean(reader.GetOrdinal("es_predeterminado")),
            UltimoUsoUtc = reader.IsDBNull(reader.GetOrdinal("ultimo_uso_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("ultimo_uso_utc")),
            FechaEnrolamientoUtc = reader.GetDateTime(reader.GetOrdinal("fecha_enrolamiento_utc")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<FactorMfaUsuarioDto?> ObtenerAsync(long idFactorMfaUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_factor_mfa_usuario", SqlDbType.BigInt, idFactorMfaUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new FactorMfaUsuarioDto
            {
            IdFactorMfaUsuario = reader.GetInt64(reader.GetOrdinal("id_factor_mfa_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoFactorMfa = reader.GetInt16(reader.GetOrdinal("id_tipo_factor_mfa")),
            Etiqueta = reader.IsDBNull(reader.GetOrdinal("etiqueta")) ? null : reader.GetString(reader.GetOrdinal("etiqueta")),
            DestinoEnmascarado = reader.IsDBNull(reader.GetOrdinal("destino_enmascarado")) ? null : reader.GetString(reader.GetOrdinal("destino_enmascarado")),
            ReferenciaSecreto = reader.IsDBNull(reader.GetOrdinal("referencia_secreto")) ? null : reader.GetString(reader.GetOrdinal("referencia_secreto")),
            SecretoCifrado = reader.IsDBNull(reader.GetOrdinal("secreto_cifrado")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("secreto_cifrado")),
            ConfiguracionJson = reader.IsDBNull(reader.GetOrdinal("configuracion_json")) ? null : reader.GetString(reader.GetOrdinal("configuracion_json")),
            Verificado = reader.GetBoolean(reader.GetOrdinal("verificado")),
            EsPredeterminado = reader.GetBoolean(reader.GetOrdinal("es_predeterminado")),
            UltimoUsoUtc = reader.IsDBNull(reader.GetOrdinal("ultimo_uso_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("ultimo_uso_utc")),
            FechaEnrolamientoUtc = reader.GetDateTime(reader.GetOrdinal("fecha_enrolamiento_utc")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(FactorMfaUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_factor_mfa", SqlDbType.SmallInt, dto.IdTipoFactorMfa));
        command.Parameters.Add(CreateParameter("@etiqueta", SqlDbType.NVarChar, dto.Etiqueta, 100));
        command.Parameters.Add(CreateParameter("@destino_enmascarado", SqlDbType.NVarChar, dto.DestinoEnmascarado, 120));
        command.Parameters.Add(CreateParameter("@referencia_secreto", SqlDbType.NVarChar, dto.ReferenciaSecreto, 300));
        command.Parameters.Add(CreateParameter("@secreto_cifrado", SqlDbType.VarBinary, dto.SecretoCifrado, 512));
        command.Parameters.Add(CreateParameter("@configuracion_json", SqlDbType.NVarChar, dto.ConfiguracionJson, -1));
        command.Parameters.Add(CreateParameter("@verificado", SqlDbType.Bit, dto.Verificado));
        command.Parameters.Add(CreateParameter("@es_predeterminado", SqlDbType.Bit, dto.EsPredeterminado));
        command.Parameters.Add(CreateParameter("@ultimo_uso_utc", SqlDbType.DateTime2, dto.UltimoUsoUtc));
        command.Parameters.Add(CreateParameter("@fecha_enrolamiento_utc", SqlDbType.DateTime2, dto.FechaEnrolamientoUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(FactorMfaUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_factor_mfa_usuario", SqlDbType.BigInt, dto.IdFactorMfaUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_factor_mfa", SqlDbType.SmallInt, dto.IdTipoFactorMfa));
        command.Parameters.Add(CreateParameter("@etiqueta", SqlDbType.NVarChar, dto.Etiqueta, 100));
        command.Parameters.Add(CreateParameter("@destino_enmascarado", SqlDbType.NVarChar, dto.DestinoEnmascarado, 120));
        command.Parameters.Add(CreateParameter("@referencia_secreto", SqlDbType.NVarChar, dto.ReferenciaSecreto, 300));
        command.Parameters.Add(CreateParameter("@secreto_cifrado", SqlDbType.VarBinary, dto.SecretoCifrado, 512));
        command.Parameters.Add(CreateParameter("@configuracion_json", SqlDbType.NVarChar, dto.ConfiguracionJson, -1));
        command.Parameters.Add(CreateParameter("@verificado", SqlDbType.Bit, dto.Verificado));
        command.Parameters.Add(CreateParameter("@es_predeterminado", SqlDbType.Bit, dto.EsPredeterminado));
        command.Parameters.Add(CreateParameter("@ultimo_uso_utc", SqlDbType.DateTime2, dto.UltimoUsoUtc));
        command.Parameters.Add(CreateParameter("@fecha_enrolamiento_utc", SqlDbType.DateTime2, dto.FechaEnrolamientoUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idFactorMfaUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_factor_mfa_usuario", SqlDbType.BigInt, idFactorMfaUsuario));
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
