using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Dbo;
using Secure.Platform.Data.Repositories.Interfaces.Dbo;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Dbo;

/// <summary>
/// Repositorio ADO.NET para dbo.sysdiagrams con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SysdiagramsRepository : ISysdiagramsRepository
{
    private const string SpListar = "dbo.usp_sysdiagrams_listar";
    private const string SpObtener = "dbo.usp_sysdiagrams_obtener";
    private const string SpCrear = "dbo.usp_sysdiagrams_crear";
    private const string SpActualizar = "dbo.usp_sysdiagrams_actualizar";
    private const string SpDesactivar = "dbo.usp_sysdiagrams_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public SysdiagramsRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<SysdiagramsDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<SysdiagramsDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new SysdiagramsDto
            {
            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
            PrincipalId = reader.GetInt32(reader.GetOrdinal("principal_id")),
            DiagramId = reader.GetInt32(reader.GetOrdinal("diagram_id")),
            Version = reader.IsDBNull(reader.GetOrdinal("version")) ? null : reader.GetInt32(reader.GetOrdinal("version")),
            Definition = reader.IsDBNull(reader.GetOrdinal("definition")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("definition"))
            });
        }
        return result;
    }

    public async Task<SysdiagramsDto?> ObtenerAsync(string name, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@name", SqlDbType.NVarChar, name, 128));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new SysdiagramsDto
            {
            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? string.Empty : reader.GetString(reader.GetOrdinal("name")),
            PrincipalId = reader.GetInt32(reader.GetOrdinal("principal_id")),
            DiagramId = reader.GetInt32(reader.GetOrdinal("diagram_id")),
            Version = reader.IsDBNull(reader.GetOrdinal("version")) ? null : reader.GetInt32(reader.GetOrdinal("version")),
            Definition = reader.IsDBNull(reader.GetOrdinal("definition")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("definition"))
            };
        }
        return null;
    }

    public async Task<string> CrearAsync(SysdiagramsDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@name", SqlDbType.NVarChar, dto.Name, 128));
        command.Parameters.Add(CreateParameter("@principal_id", SqlDbType.Int, dto.PrincipalId));
        command.Parameters.Add(CreateParameter("@diagram_id", SqlDbType.Int, dto.DiagramId));
        command.Parameters.Add(CreateParameter("@version", SqlDbType.Int, dto.Version));
        command.Parameters.Add(CreateParameter("@definition", SqlDbType.VarBinary, dto.Definition, -1));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> ActualizarAsync(SysdiagramsDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@name", SqlDbType.NVarChar, dto.Name, 128));
        command.Parameters.Add(CreateParameter("@principal_id", SqlDbType.Int, dto.PrincipalId));
        command.Parameters.Add(CreateParameter("@diagram_id", SqlDbType.Int, dto.DiagramId));
        command.Parameters.Add(CreateParameter("@version", SqlDbType.Int, dto.Version));
        command.Parameters.Add(CreateParameter("@definition", SqlDbType.VarBinary, dto.Definition, -1));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(string name, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@name", SqlDbType.NVarChar, name, 128));
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
