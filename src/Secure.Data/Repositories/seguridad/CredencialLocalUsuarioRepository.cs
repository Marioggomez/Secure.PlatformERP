using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.credencial_local_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CredencialLocalUsuarioRepository : ICredencialLocalUsuarioRepository
{
    private const string SpListar = "seguridad.usp_credencial_local_usuario_listar";
    private const string SpObtener = "seguridad.usp_credencial_local_usuario_obtener";
    private const string SpCrear = "seguridad.usp_credencial_local_usuario_crear";
    private const string SpActualizar = "seguridad.usp_credencial_local_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_credencial_local_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public CredencialLocalUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<CredencialLocalUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<CredencialLocalUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new CredencialLocalUsuarioDto
            {
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            HashClave = reader.IsDBNull(reader.GetOrdinal("hash_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("hash_clave")),
            SaltClave = reader.IsDBNull(reader.GetOrdinal("salt_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("salt_clave")),
            AlgoritmoClave = reader.IsDBNull(reader.GetOrdinal("algoritmo_clave")) ? string.Empty : reader.GetString(reader.GetOrdinal("algoritmo_clave")),
            IteracionesClave = reader.GetInt32(reader.GetOrdinal("iteraciones_clave")),
            CambioClaveUtc = reader.GetDateTime(reader.GetOrdinal("cambio_clave_utc")),
            DebeCambiarClave = reader.GetBoolean(reader.GetOrdinal("debe_cambiar_clave")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<CredencialLocalUsuarioDto?> ObtenerAsync(long idUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new CredencialLocalUsuarioDto
            {
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            HashClave = reader.IsDBNull(reader.GetOrdinal("hash_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("hash_clave")),
            SaltClave = reader.IsDBNull(reader.GetOrdinal("salt_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("salt_clave")),
            AlgoritmoClave = reader.IsDBNull(reader.GetOrdinal("algoritmo_clave")) ? string.Empty : reader.GetString(reader.GetOrdinal("algoritmo_clave")),
            IteracionesClave = reader.GetInt32(reader.GetOrdinal("iteraciones_clave")),
            CambioClaveUtc = reader.GetDateTime(reader.GetOrdinal("cambio_clave_utc")),
            DebeCambiarClave = reader.GetBoolean(reader.GetOrdinal("debe_cambiar_clave")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(CredencialLocalUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@hash_clave", SqlDbType.VarBinary, dto.HashClave, 128));
        command.Parameters.Add(CreateParameter("@salt_clave", SqlDbType.VarBinary, dto.SaltClave, 32));
        command.Parameters.Add(CreateParameter("@algoritmo_clave", SqlDbType.VarChar, dto.AlgoritmoClave, 30));
        command.Parameters.Add(CreateParameter("@iteraciones_clave", SqlDbType.Int, dto.IteracionesClave));
        command.Parameters.Add(CreateParameter("@cambio_clave_utc", SqlDbType.DateTime2, dto.CambioClaveUtc));
        command.Parameters.Add(CreateParameter("@debe_cambiar_clave", SqlDbType.Bit, dto.DebeCambiarClave));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(CredencialLocalUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@hash_clave", SqlDbType.VarBinary, dto.HashClave, 128));
        command.Parameters.Add(CreateParameter("@salt_clave", SqlDbType.VarBinary, dto.SaltClave, 32));
        command.Parameters.Add(CreateParameter("@algoritmo_clave", SqlDbType.VarChar, dto.AlgoritmoClave, 30));
        command.Parameters.Add(CreateParameter("@iteraciones_clave", SqlDbType.Int, dto.IteracionesClave));
        command.Parameters.Add(CreateParameter("@cambio_clave_utc", SqlDbType.DateTime2, dto.CambioClaveUtc));
        command.Parameters.Add(CreateParameter("@debe_cambiar_clave", SqlDbType.Bit, dto.DebeCambiarClave));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
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
