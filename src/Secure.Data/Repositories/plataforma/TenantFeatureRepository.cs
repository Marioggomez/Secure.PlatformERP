using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.tenant_feature con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TenantFeatureRepository : ITenantFeatureRepository
{
    private const string SpListar = "plataforma.usp_tenant_feature_listar";
    private const string SpObtener = "plataforma.usp_tenant_feature_obtener";
    private const string SpCrear = "plataforma.usp_tenant_feature_crear";
    private const string SpActualizar = "plataforma.usp_tenant_feature_actualizar";
    private const string SpDesactivar = "plataforma.usp_tenant_feature_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public TenantFeatureRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<TenantFeatureDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<TenantFeatureDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new TenantFeatureDto
            {
            IdTenantFeature = reader.GetInt64(reader.GetOrdinal("id_tenant_feature")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdFeature = reader.IsDBNull(reader.GetOrdinal("id_feature")) ? null : reader.GetInt64(reader.GetOrdinal("id_feature")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }
        return result;
    }

    public async Task<TenantFeatureDto?> ObtenerAsync(long idTenantFeature, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_tenant_feature", SqlDbType.BigInt, idTenantFeature));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new TenantFeatureDto
            {
            IdTenantFeature = reader.GetInt64(reader.GetOrdinal("id_tenant_feature")),
            IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdFeature = reader.IsDBNull(reader.GetOrdinal("id_feature")) ? null : reader.GetInt64(reader.GetOrdinal("id_feature")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(TenantFeatureDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_feature", SqlDbType.BigInt, dto.IdFeature));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(TenantFeatureDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_tenant_feature", SqlDbType.BigInt, dto.IdTenantFeature));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_feature", SqlDbType.BigInt, dto.IdFeature));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idTenantFeature, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_tenant_feature", SqlDbType.BigInt, idTenantFeature));
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
