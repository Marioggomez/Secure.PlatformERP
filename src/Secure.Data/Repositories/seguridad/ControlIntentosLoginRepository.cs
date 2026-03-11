using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.control_intentos_login con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ControlIntentosLoginRepository : IControlIntentosLoginRepository
{
    private const string SpListar = "seguridad.usp_control_intentos_login_listar";
    private const string SpObtener = "seguridad.usp_control_intentos_login_obtener";
    private const string SpCrear = "seguridad.usp_control_intentos_login_crear";
    private const string SpActualizar = "seguridad.usp_control_intentos_login_actualizar";
    private const string SpDesactivar = "seguridad.usp_control_intentos_login_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ControlIntentosLoginRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ControlIntentosLoginDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ControlIntentosLoginDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ControlIntentosLoginDto
            {
            IdControlIntento = reader.GetInt64(reader.GetOrdinal("id_control_intento")),
            LoginUsuario = reader.IsDBNull(reader.GetOrdinal("login_usuario")) ? null : reader.GetString(reader.GetOrdinal("login_usuario")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Intentos = reader.IsDBNull(reader.GetOrdinal("intentos")) ? null : reader.GetInt32(reader.GetOrdinal("intentos")),
            FechaUltimoIntento = reader.IsDBNull(reader.GetOrdinal("fecha_ultimo_intento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_ultimo_intento")),
            BloqueadoHasta = reader.IsDBNull(reader.GetOrdinal("bloqueado_hasta")) ? null : reader.GetDateTime(reader.GetOrdinal("bloqueado_hasta"))
            });
        }
        return result;
    }

    public async Task<ControlIntentosLoginDto?> ObtenerAsync(long idControlIntento, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_control_intento", SqlDbType.BigInt, idControlIntento));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ControlIntentosLoginDto
            {
            IdControlIntento = reader.GetInt64(reader.GetOrdinal("id_control_intento")),
            LoginUsuario = reader.IsDBNull(reader.GetOrdinal("login_usuario")) ? null : reader.GetString(reader.GetOrdinal("login_usuario")),
            Ip = reader.IsDBNull(reader.GetOrdinal("ip")) ? null : reader.GetString(reader.GetOrdinal("ip")),
            Intentos = reader.IsDBNull(reader.GetOrdinal("intentos")) ? null : reader.GetInt32(reader.GetOrdinal("intentos")),
            FechaUltimoIntento = reader.IsDBNull(reader.GetOrdinal("fecha_ultimo_intento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_ultimo_intento")),
            BloqueadoHasta = reader.IsDBNull(reader.GetOrdinal("bloqueado_hasta")) ? null : reader.GetDateTime(reader.GetOrdinal("bloqueado_hasta"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ControlIntentosLoginDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@login_usuario", SqlDbType.VarChar, dto.LoginUsuario, 150));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@intentos", SqlDbType.Int, dto.Intentos));
        command.Parameters.Add(CreateParameter("@fecha_ultimo_intento", SqlDbType.DateTime2, dto.FechaUltimoIntento));
        command.Parameters.Add(CreateParameter("@bloqueado_hasta", SqlDbType.DateTime2, dto.BloqueadoHasta));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ControlIntentosLoginDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_control_intento", SqlDbType.BigInt, dto.IdControlIntento));
        command.Parameters.Add(CreateParameter("@login_usuario", SqlDbType.VarChar, dto.LoginUsuario, 150));
        command.Parameters.Add(CreateParameter("@ip", SqlDbType.VarChar, dto.Ip, 50));
        command.Parameters.Add(CreateParameter("@intentos", SqlDbType.Int, dto.Intentos));
        command.Parameters.Add(CreateParameter("@fecha_ultimo_intento", SqlDbType.DateTime2, dto.FechaUltimoIntento));
        command.Parameters.Add(CreateParameter("@bloqueado_hasta", SqlDbType.DateTime2, dto.BloqueadoHasta));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idControlIntento, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_control_intento", SqlDbType.BigInt, idControlIntento));
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
