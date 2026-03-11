using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.contador_rate_limit con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ContadorRateLimitRepository : IContadorRateLimitRepository
{
    private const string SpListar = "seguridad.usp_contador_rate_limit_listar";
    private const string SpObtener = "seguridad.usp_contador_rate_limit_obtener";
    private const string SpCrear = "seguridad.usp_contador_rate_limit_crear";
    private const string SpActualizar = "seguridad.usp_contador_rate_limit_actualizar";
    private const string SpDesactivar = "seguridad.usp_contador_rate_limit_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ContadorRateLimitRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ContadorRateLimitDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ContadorRateLimitDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ContadorRateLimitDto
            {
            IdContadorRateLimit = reader.GetInt64(reader.GetOrdinal("id_contador_rate_limit")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Ambito = reader.IsDBNull(reader.GetOrdinal("ambito")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambito")),
            Llave = reader.IsDBNull(reader.GetOrdinal("llave")) ? string.Empty : reader.GetString(reader.GetOrdinal("llave")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("endpoint")),
            InicioVentanaUtc = reader.GetDateTime(reader.GetOrdinal("inicio_ventana_utc")),
            Conteo = reader.GetInt32(reader.GetOrdinal("conteo"))
            });
        }
        return result;
    }

    public async Task<ContadorRateLimitDto?> ObtenerAsync(long idContadorRateLimit, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_contador_rate_limit", SqlDbType.BigInt, idContadorRateLimit));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ContadorRateLimitDto
            {
            IdContadorRateLimit = reader.GetInt64(reader.GetOrdinal("id_contador_rate_limit")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Ambito = reader.IsDBNull(reader.GetOrdinal("ambito")) ? string.Empty : reader.GetString(reader.GetOrdinal("ambito")),
            Llave = reader.IsDBNull(reader.GetOrdinal("llave")) ? string.Empty : reader.GetString(reader.GetOrdinal("llave")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? string.Empty : reader.GetString(reader.GetOrdinal("endpoint")),
            InicioVentanaUtc = reader.GetDateTime(reader.GetOrdinal("inicio_ventana_utc")),
            Conteo = reader.GetInt32(reader.GetOrdinal("conteo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ContadorRateLimitDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@ambito", SqlDbType.NVarChar, dto.Ambito, 20));
        command.Parameters.Add(CreateParameter("@llave", SqlDbType.NVarChar, dto.Llave, 200));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.NVarChar, dto.Endpoint, 200));
        command.Parameters.Add(CreateParameter("@inicio_ventana_utc", SqlDbType.DateTime2, dto.InicioVentanaUtc));
        command.Parameters.Add(CreateParameter("@conteo", SqlDbType.Int, dto.Conteo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ContadorRateLimitDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_contador_rate_limit", SqlDbType.BigInt, dto.IdContadorRateLimit));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@ambito", SqlDbType.NVarChar, dto.Ambito, 20));
        command.Parameters.Add(CreateParameter("@llave", SqlDbType.NVarChar, dto.Llave, 200));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.NVarChar, dto.Endpoint, 200));
        command.Parameters.Add(CreateParameter("@inicio_ventana_utc", SqlDbType.DateTime2, dto.InicioVentanaUtc));
        command.Parameters.Add(CreateParameter("@conteo", SqlDbType.Int, dto.Conteo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idContadorRateLimit, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_contador_rate_limit", SqlDbType.BigInt, idContadorRateLimit));
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
