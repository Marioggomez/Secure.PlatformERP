using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.historial_clave_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class HistorialClaveUsuarioRepository : IHistorialClaveUsuarioRepository
{
    private const string SpListar = "seguridad.usp_historial_clave_usuario_listar";
    private const string SpObtener = "seguridad.usp_historial_clave_usuario_obtener";
    private const string SpCrear = "seguridad.usp_historial_clave_usuario_crear";
    private const string SpActualizar = "seguridad.usp_historial_clave_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_historial_clave_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public HistorialClaveUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<HistorialClaveUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<HistorialClaveUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new HistorialClaveUsuarioDto
            {
            IdHistorialClaveUsuario = reader.GetInt64(reader.GetOrdinal("id_historial_clave_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            HashClave = reader.IsDBNull(reader.GetOrdinal("hash_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("hash_clave")),
            SaltClave = reader.IsDBNull(reader.GetOrdinal("salt_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("salt_clave")),
            AlgoritmoClave = reader.IsDBNull(reader.GetOrdinal("algoritmo_clave")) ? string.Empty : reader.GetString(reader.GetOrdinal("algoritmo_clave")),
            IteracionesClave = reader.GetInt32(reader.GetOrdinal("iteraciones_clave")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<HistorialClaveUsuarioDto?> ObtenerAsync(long idHistorialClaveUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_historial_clave_usuario", SqlDbType.BigInt, idHistorialClaveUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new HistorialClaveUsuarioDto
            {
            IdHistorialClaveUsuario = reader.GetInt64(reader.GetOrdinal("id_historial_clave_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            HashClave = reader.IsDBNull(reader.GetOrdinal("hash_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("hash_clave")),
            SaltClave = reader.IsDBNull(reader.GetOrdinal("salt_clave")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("salt_clave")),
            AlgoritmoClave = reader.IsDBNull(reader.GetOrdinal("algoritmo_clave")) ? string.Empty : reader.GetString(reader.GetOrdinal("algoritmo_clave")),
            IteracionesClave = reader.GetInt32(reader.GetOrdinal("iteraciones_clave")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(HistorialClaveUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@hash_clave", SqlDbType.VarBinary, dto.HashClave, 128));
        command.Parameters.Add(CreateParameter("@salt_clave", SqlDbType.VarBinary, dto.SaltClave, 32));
        command.Parameters.Add(CreateParameter("@algoritmo_clave", SqlDbType.VarChar, dto.AlgoritmoClave, 30));
        command.Parameters.Add(CreateParameter("@iteraciones_clave", SqlDbType.Int, dto.IteracionesClave));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(HistorialClaveUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_historial_clave_usuario", SqlDbType.BigInt, dto.IdHistorialClaveUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@hash_clave", SqlDbType.VarBinary, dto.HashClave, 128));
        command.Parameters.Add(CreateParameter("@salt_clave", SqlDbType.VarBinary, dto.SaltClave, 32));
        command.Parameters.Add(CreateParameter("@algoritmo_clave", SqlDbType.VarChar, dto.AlgoritmoClave, 30));
        command.Parameters.Add(CreateParameter("@iteraciones_clave", SqlDbType.Int, dto.IteracionesClave));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idHistorialClaveUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_historial_clave_usuario", SqlDbType.BigInt, idHistorialClaveUsuario));
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
