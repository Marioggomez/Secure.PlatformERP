using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.ip_bloqueada con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IpBloqueadaRepository : IIpBloqueadaRepository
{
    private const string SpListar = "seguridad.usp_ip_bloqueada_listar";
    private const string SpObtener = "seguridad.usp_ip_bloqueada_obtener";
    private const string SpCrear = "seguridad.usp_ip_bloqueada_crear";
    private const string SpActualizar = "seguridad.usp_ip_bloqueada_actualizar";
    private const string SpDesactivar = "seguridad.usp_ip_bloqueada_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public IpBloqueadaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<IpBloqueadaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<IpBloqueadaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new IpBloqueadaDto
            {
            IdIpBloqueada = reader.GetInt64(reader.GetOrdinal("id_ip_bloqueada")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            FechaBloqueo = reader.IsDBNull(reader.GetOrdinal("fecha_bloqueo")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_bloqueo")),
            FechaExpiracion = reader.IsDBNull(reader.GetOrdinal("fecha_expiracion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_expiracion"))
            });
        }
        return result;
    }

    public async Task<IpBloqueadaDto?> ObtenerAsync(long idIpBloqueada, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_ip_bloqueada", SqlDbType.BigInt, idIpBloqueada));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new IpBloqueadaDto
            {
            IdIpBloqueada = reader.GetInt64(reader.GetOrdinal("id_ip_bloqueada")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            FechaBloqueo = reader.IsDBNull(reader.GetOrdinal("fecha_bloqueo")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_bloqueo")),
            FechaExpiracion = reader.IsDBNull(reader.GetOrdinal("fecha_expiracion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_expiracion"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(IpBloqueadaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.VarChar, dto.Motivo, 200));
        command.Parameters.Add(CreateParameter("@fecha_bloqueo", SqlDbType.DateTime2, dto.FechaBloqueo));
        command.Parameters.Add(CreateParameter("@fecha_expiracion", SqlDbType.DateTime2, dto.FechaExpiracion));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(IpBloqueadaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_ip_bloqueada", SqlDbType.BigInt, dto.IdIpBloqueada));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.VarChar, dto.Motivo, 200));
        command.Parameters.Add(CreateParameter("@fecha_bloqueo", SqlDbType.DateTime2, dto.FechaBloqueo));
        command.Parameters.Add(CreateParameter("@fecha_expiracion", SqlDbType.DateTime2, dto.FechaExpiracion));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idIpBloqueada, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_ip_bloqueada", SqlDbType.BigInt, idIpBloqueada));
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
