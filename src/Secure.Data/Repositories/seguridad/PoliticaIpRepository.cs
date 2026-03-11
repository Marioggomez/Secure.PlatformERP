using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.politica_ip con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaIpRepository : IPoliticaIpRepository
{
    private const string SpListar = "seguridad.usp_politica_ip_listar";
    private const string SpObtener = "seguridad.usp_politica_ip_obtener";
    private const string SpCrear = "seguridad.usp_politica_ip_crear";
    private const string SpActualizar = "seguridad.usp_politica_ip_actualizar";
    private const string SpDesactivar = "seguridad.usp_politica_ip_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PoliticaIpRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PoliticaIpDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PoliticaIpDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PoliticaIpDto
            {
            IdPoliticaIp = reader.GetInt64(reader.GetOrdinal("id_politica_ip")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IpOCidr = reader.IsDBNull(reader.GetOrdinal("ip_o_cidr")) ? string.Empty : reader.GetString(reader.GetOrdinal("ip_o_cidr")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Prioridad = reader.GetInt32(reader.GetOrdinal("prioridad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<PoliticaIpDto?> ObtenerAsync(long idPoliticaIp, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_politica_ip", SqlDbType.BigInt, idPoliticaIp));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PoliticaIpDto
            {
            IdPoliticaIp = reader.GetInt64(reader.GetOrdinal("id_politica_ip")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IpOCidr = reader.IsDBNull(reader.GetOrdinal("ip_o_cidr")) ? string.Empty : reader.GetString(reader.GetOrdinal("ip_o_cidr")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Prioridad = reader.GetInt32(reader.GetOrdinal("prioridad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PoliticaIpDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@ip_o_cidr", SqlDbType.NVarChar, dto.IpOCidr, 64));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.VarChar, dto.Accion, 10));
        command.Parameters.Add(CreateParameter("@prioridad", SqlDbType.Int, dto.Prioridad));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PoliticaIpDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_politica_ip", SqlDbType.BigInt, dto.IdPoliticaIp));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@ip_o_cidr", SqlDbType.NVarChar, dto.IpOCidr, 64));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.VarChar, dto.Accion, 10));
        command.Parameters.Add(CreateParameter("@prioridad", SqlDbType.Int, dto.Prioridad));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idPoliticaIp, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_politica_ip", SqlDbType.BigInt, idPoliticaIp));
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
