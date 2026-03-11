using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.version_esquema con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class VersionEsquemaRepository : IVersionEsquemaRepository
{
    private const string SpListar = "plataforma.usp_version_esquema_listar";
    private const string SpObtener = "plataforma.usp_version_esquema_obtener";
    private const string SpCrear = "plataforma.usp_version_esquema_crear";
    private const string SpActualizar = "plataforma.usp_version_esquema_actualizar";
    private const string SpDesactivar = "plataforma.usp_version_esquema_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public VersionEsquemaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<VersionEsquemaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<VersionEsquemaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new VersionEsquemaDto
            {
            IdVersionEsquema = reader.GetInt64(reader.GetOrdinal("id_version_esquema")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("componente")),
            VersionCodigo = reader.IsDBNull(reader.GetOrdinal("version_codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("version_codigo")),
            Checksum = reader.IsDBNull(reader.GetOrdinal("checksum")) ? null : reader.GetString(reader.GetOrdinal("checksum")),
            InstaladoPor = reader.IsDBNull(reader.GetOrdinal("instalado_por")) ? null : reader.GetString(reader.GetOrdinal("instalado_por")),
            InstaladoUtc = reader.GetDateTime(reader.GetOrdinal("instalado_utc")),
            Notas = reader.IsDBNull(reader.GetOrdinal("notas")) ? null : reader.GetString(reader.GetOrdinal("notas"))
            });
        }
        return result;
    }

    public async Task<VersionEsquemaDto?> ObtenerAsync(long idVersionEsquema, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_version_esquema", SqlDbType.BigInt, idVersionEsquema));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new VersionEsquemaDto
            {
            IdVersionEsquema = reader.GetInt64(reader.GetOrdinal("id_version_esquema")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("componente")),
            VersionCodigo = reader.IsDBNull(reader.GetOrdinal("version_codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("version_codigo")),
            Checksum = reader.IsDBNull(reader.GetOrdinal("checksum")) ? null : reader.GetString(reader.GetOrdinal("checksum")),
            InstaladoPor = reader.IsDBNull(reader.GetOrdinal("instalado_por")) ? null : reader.GetString(reader.GetOrdinal("instalado_por")),
            InstaladoUtc = reader.GetDateTime(reader.GetOrdinal("instalado_utc")),
            Notas = reader.IsDBNull(reader.GetOrdinal("notas")) ? null : reader.GetString(reader.GetOrdinal("notas"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(VersionEsquemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 100));
        command.Parameters.Add(CreateParameter("@version_codigo", SqlDbType.NVarChar, dto.VersionCodigo, 50));
        command.Parameters.Add(CreateParameter("@checksum", SqlDbType.NVarChar, dto.Checksum, 128));
        command.Parameters.Add(CreateParameter("@instalado_por", SqlDbType.NVarChar, dto.InstaladoPor, 100));
        command.Parameters.Add(CreateParameter("@instalado_utc", SqlDbType.DateTime2, dto.InstaladoUtc));
        command.Parameters.Add(CreateParameter("@notas", SqlDbType.NVarChar, dto.Notas, 500));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(VersionEsquemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_version_esquema", SqlDbType.BigInt, dto.IdVersionEsquema));
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 100));
        command.Parameters.Add(CreateParameter("@version_codigo", SqlDbType.NVarChar, dto.VersionCodigo, 50));
        command.Parameters.Add(CreateParameter("@checksum", SqlDbType.NVarChar, dto.Checksum, 128));
        command.Parameters.Add(CreateParameter("@instalado_por", SqlDbType.NVarChar, dto.InstaladoPor, 100));
        command.Parameters.Add(CreateParameter("@instalado_utc", SqlDbType.DateTime2, dto.InstaladoUtc));
        command.Parameters.Add(CreateParameter("@notas", SqlDbType.NVarChar, dto.Notas, 500));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idVersionEsquema, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_version_esquema", SqlDbType.BigInt, idVersionEsquema));
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
