using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.sesion_usuario_historial con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SesionUsuarioHistorialRepository : ISesionUsuarioHistorialRepository
{
    private const string SpListar = "seguridad.usp_sesion_usuario_historial_listar";
    private const string SpObtener = "seguridad.usp_sesion_usuario_historial_obtener";
    private const string SpCrear = "seguridad.usp_sesion_usuario_historial_crear";
    private const string SpActualizar = "seguridad.usp_sesion_usuario_historial_actualizar";
    private const string SpDesactivar = "seguridad.usp_sesion_usuario_historial_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public SesionUsuarioHistorialRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<SesionUsuarioHistorialDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<SesionUsuarioHistorialDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new SesionUsuarioHistorialDto
            {
            IdHistorial = reader.GetInt64(reader.GetOrdinal("id_historial")),
            IdSesion = reader.IsDBNull(reader.GetOrdinal("id_sesion")) ? null : reader.GetInt64(reader.GetOrdinal("id_sesion")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            FechaInicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
            FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Dispositivo = reader.IsDBNull(reader.GetOrdinal("dispositivo")) ? null : reader.GetString(reader.GetOrdinal("dispositivo")),
            MotivoCierre = reader.IsDBNull(reader.GetOrdinal("motivo_cierre")) ? null : reader.GetString(reader.GetOrdinal("motivo_cierre"))
            });
        }
        return result;
    }

    public async Task<SesionUsuarioHistorialDto?> ObtenerAsync(long idHistorial, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_historial", SqlDbType.BigInt, idHistorial));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new SesionUsuarioHistorialDto
            {
            IdHistorial = reader.GetInt64(reader.GetOrdinal("id_historial")),
            IdSesion = reader.IsDBNull(reader.GetOrdinal("id_sesion")) ? null : reader.GetInt64(reader.GetOrdinal("id_sesion")),
            IdUsuario = reader.IsDBNull(reader.GetOrdinal("id_usuario")) ? null : reader.GetInt64(reader.GetOrdinal("id_usuario")),
            FechaInicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
            FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Dispositivo = reader.IsDBNull(reader.GetOrdinal("dispositivo")) ? null : reader.GetString(reader.GetOrdinal("dispositivo")),
            MotivoCierre = reader.IsDBNull(reader.GetOrdinal("motivo_cierre")) ? null : reader.GetString(reader.GetOrdinal("motivo_cierre"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(SesionUsuarioHistorialDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_sesion", SqlDbType.BigInt, dto.IdSesion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@fecha_inicio", SqlDbType.DateTime2, dto.FechaInicio));
        command.Parameters.Add(CreateParameter("@fecha_fin", SqlDbType.DateTime2, dto.FechaFin));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@dispositivo", SqlDbType.VarChar, dto.Dispositivo, 200));
        command.Parameters.Add(CreateParameter("@motivo_cierre", SqlDbType.VarChar, dto.MotivoCierre, 200));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(SesionUsuarioHistorialDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_historial", SqlDbType.BigInt, dto.IdHistorial));
        command.Parameters.Add(CreateParameter("@id_sesion", SqlDbType.BigInt, dto.IdSesion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@fecha_inicio", SqlDbType.DateTime2, dto.FechaInicio));
        command.Parameters.Add(CreateParameter("@fecha_fin", SqlDbType.DateTime2, dto.FechaFin));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@dispositivo", SqlDbType.VarChar, dto.Dispositivo, 200));
        command.Parameters.Add(CreateParameter("@motivo_cierre", SqlDbType.VarChar, dto.MotivoCierre, 200));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idHistorial, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_historial", SqlDbType.BigInt, idHistorial));
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
