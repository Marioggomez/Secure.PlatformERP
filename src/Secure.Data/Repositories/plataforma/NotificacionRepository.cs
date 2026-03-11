using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.notificacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class NotificacionRepository : INotificacionRepository
{
    private const string SpListar = "plataforma.usp_notificacion_listar";
    private const string SpObtener = "plataforma.usp_notificacion_obtener";
    private const string SpCrear = "plataforma.usp_notificacion_crear";
    private const string SpActualizar = "plataforma.usp_notificacion_actualizar";
    private const string SpDesactivar = "plataforma.usp_notificacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public NotificacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<NotificacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<NotificacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new NotificacionDto
            {
            IdNotificacion = reader.GetInt64(reader.GetOrdinal("id_notificacion")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            Titulo = reader.IsDBNull(reader.GetOrdinal("titulo")) ? null : reader.GetString(reader.GetOrdinal("titulo")),
            Mensaje = reader.IsDBNull(reader.GetOrdinal("mensaje")) ? null : reader.GetString(reader.GetOrdinal("mensaje")),
            Leida = reader.IsDBNull(reader.GetOrdinal("leida")) ? null : reader.GetBoolean(reader.GetOrdinal("leida")),
            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_creacion"))
            });
        }
        return result;
    }

    public async Task<NotificacionDto?> ObtenerAsync(long idNotificacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_notificacion", SqlDbType.BigInt, idNotificacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new NotificacionDto
            {
            IdNotificacion = reader.GetInt64(reader.GetOrdinal("id_notificacion")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            Titulo = reader.IsDBNull(reader.GetOrdinal("titulo")) ? null : reader.GetString(reader.GetOrdinal("titulo")),
            Mensaje = reader.IsDBNull(reader.GetOrdinal("mensaje")) ? null : reader.GetString(reader.GetOrdinal("mensaje")),
            Leida = reader.IsDBNull(reader.GetOrdinal("leida")) ? null : reader.GetBoolean(reader.GetOrdinal("leida")),
            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_creacion"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(NotificacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@titulo", SqlDbType.VarChar, dto.Titulo, 200));
        command.Parameters.Add(CreateParameter("@mensaje", SqlDbType.VarChar, dto.Mensaje, 2000));
        command.Parameters.Add(CreateParameter("@leida", SqlDbType.Bit, dto.Leida));
        command.Parameters.Add(CreateParameter("@fecha_creacion", SqlDbType.DateTime2, dto.FechaCreacion));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(NotificacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_notificacion", SqlDbType.BigInt, dto.IdNotificacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@titulo", SqlDbType.VarChar, dto.Titulo, 200));
        command.Parameters.Add(CreateParameter("@mensaje", SqlDbType.VarChar, dto.Mensaje, 2000));
        command.Parameters.Add(CreateParameter("@leida", SqlDbType.Bit, dto.Leida));
        command.Parameters.Add(CreateParameter("@fecha_creacion", SqlDbType.DateTime2, dto.FechaCreacion));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idNotificacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_notificacion", SqlDbType.BigInt, idNotificacion));
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
