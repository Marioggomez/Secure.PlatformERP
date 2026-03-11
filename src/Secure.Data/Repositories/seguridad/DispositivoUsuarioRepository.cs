using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.dispositivo_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DispositivoUsuarioRepository : IDispositivoUsuarioRepository
{
    private const string SpListar = "seguridad.usp_dispositivo_usuario_listar";
    private const string SpObtener = "seguridad.usp_dispositivo_usuario_obtener";
    private const string SpCrear = "seguridad.usp_dispositivo_usuario_crear";
    private const string SpActualizar = "seguridad.usp_dispositivo_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_dispositivo_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public DispositivoUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<DispositivoUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<DispositivoUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new DispositivoUsuarioDto
            {
            IdDispositivoUsuario = reader.GetInt64(reader.GetOrdinal("id_dispositivo_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            Fingerprint = reader.IsDBNull(reader.GetOrdinal("fingerprint")) ? null : reader.GetString(reader.GetOrdinal("fingerprint")),
            Navegador = reader.IsDBNull(reader.GetOrdinal("navegador")) ? null : reader.GetString(reader.GetOrdinal("navegador")),
            SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("sistema_operativo")) ? null : reader.GetString(reader.GetOrdinal("sistema_operativo")),
            IpUltimoAcceso = reader.IsDBNull(reader.GetOrdinal("ip_ultimo_acceso")) ? null : reader.GetString(reader.GetOrdinal("ip_ultimo_acceso")),
            FechaRegistro = reader.IsDBNull(reader.GetOrdinal("fecha_registro")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_registro")),
            FechaUltimoAcceso = reader.IsDBNull(reader.GetOrdinal("fecha_ultimo_acceso")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_ultimo_acceso"))
            });
        }
        return result;
    }

    public async Task<DispositivoUsuarioDto?> ObtenerAsync(long idDispositivoUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_dispositivo_usuario", SqlDbType.BigInt, idDispositivoUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new DispositivoUsuarioDto
            {
            IdDispositivoUsuario = reader.GetInt64(reader.GetOrdinal("id_dispositivo_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            Fingerprint = reader.IsDBNull(reader.GetOrdinal("fingerprint")) ? null : reader.GetString(reader.GetOrdinal("fingerprint")),
            Navegador = reader.IsDBNull(reader.GetOrdinal("navegador")) ? null : reader.GetString(reader.GetOrdinal("navegador")),
            SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("sistema_operativo")) ? null : reader.GetString(reader.GetOrdinal("sistema_operativo")),
            IpUltimoAcceso = reader.IsDBNull(reader.GetOrdinal("ip_ultimo_acceso")) ? null : reader.GetString(reader.GetOrdinal("ip_ultimo_acceso")),
            FechaRegistro = reader.IsDBNull(reader.GetOrdinal("fecha_registro")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_registro")),
            FechaUltimoAcceso = reader.IsDBNull(reader.GetOrdinal("fecha_ultimo_acceso")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_ultimo_acceso"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(DispositivoUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@fingerprint", SqlDbType.VarChar, dto.Fingerprint, 200));
        command.Parameters.Add(CreateParameter("@navegador", SqlDbType.VarChar, dto.Navegador, 200));
        command.Parameters.Add(CreateParameter("@sistema_operativo", SqlDbType.VarChar, dto.SistemaOperativo, 200));
        command.Parameters.Add(CreateParameter("@ip_ultimo_acceso", SqlDbType.VarChar, dto.IpUltimoAcceso, 50));
        command.Parameters.Add(CreateParameter("@fecha_registro", SqlDbType.DateTime2, dto.FechaRegistro));
        command.Parameters.Add(CreateParameter("@fecha_ultimo_acceso", SqlDbType.DateTime2, dto.FechaUltimoAcceso));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(DispositivoUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_dispositivo_usuario", SqlDbType.BigInt, dto.IdDispositivoUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@fingerprint", SqlDbType.VarChar, dto.Fingerprint, 200));
        command.Parameters.Add(CreateParameter("@navegador", SqlDbType.VarChar, dto.Navegador, 200));
        command.Parameters.Add(CreateParameter("@sistema_operativo", SqlDbType.VarChar, dto.SistemaOperativo, 200));
        command.Parameters.Add(CreateParameter("@ip_ultimo_acceso", SqlDbType.VarChar, dto.IpUltimoAcceso, 50));
        command.Parameters.Add(CreateParameter("@fecha_registro", SqlDbType.DateTime2, dto.FechaRegistro));
        command.Parameters.Add(CreateParameter("@fecha_ultimo_acceso", SqlDbType.DateTime2, dto.FechaUltimoAcceso));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idDispositivoUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_dispositivo_usuario", SqlDbType.BigInt, idDispositivoUsuario));
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
