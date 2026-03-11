using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.evento_sistema con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EventoSistemaRepository : IEventoSistemaRepository
{
    private const string SpListar = "observabilidad.usp_evento_sistema_listar";
    private const string SpObtener = "observabilidad.usp_evento_sistema_obtener";
    private const string SpCrear = "observabilidad.usp_evento_sistema_crear";
    private const string SpActualizar = "observabilidad.usp_evento_sistema_actualizar";
    private const string SpDesactivar = "observabilidad.usp_evento_sistema_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public EventoSistemaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<EventoSistemaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<EventoSistemaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new EventoSistemaDto
            {
            IdEventoSistema = reader.GetInt64(reader.GetOrdinal("id_evento_sistema")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            TipoEvento = reader.IsDBNull(reader.GetOrdinal("tipo_evento")) ? null : reader.GetString(reader.GetOrdinal("tipo_evento")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            });
        }
        return result;
    }

    public async Task<EventoSistemaDto?> ObtenerAsync(long idEventoSistema, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_evento_sistema", SqlDbType.BigInt, idEventoSistema));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new EventoSistemaDto
            {
            IdEventoSistema = reader.GetInt64(reader.GetOrdinal("id_evento_sistema")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            TipoEvento = reader.IsDBNull(reader.GetOrdinal("tipo_evento")) ? null : reader.GetString(reader.GetOrdinal("tipo_evento")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(EventoSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@tipo_evento", SqlDbType.VarChar, dto.TipoEvento, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 1000));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(EventoSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_evento_sistema", SqlDbType.BigInt, dto.IdEventoSistema));
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@tipo_evento", SqlDbType.VarChar, dto.TipoEvento, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 1000));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 150));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idEventoSistema, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_evento_sistema", SqlDbType.BigInt, idEventoSistema));
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
