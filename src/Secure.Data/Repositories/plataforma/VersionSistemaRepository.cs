using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.version_sistema con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class VersionSistemaRepository : IVersionSistemaRepository
{
    private const string SpListar = "plataforma.usp_version_sistema_listar";
    private const string SpObtener = "plataforma.usp_version_sistema_obtener";
    private const string SpCrear = "plataforma.usp_version_sistema_crear";
    private const string SpActualizar = "plataforma.usp_version_sistema_actualizar";
    private const string SpDesactivar = "plataforma.usp_version_sistema_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public VersionSistemaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<VersionSistemaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<VersionSistemaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new VersionSistemaDto
            {
            IdVersionSistema = reader.GetInt32(reader.GetOrdinal("id_version_sistema")),
            Version = reader.IsDBNull(reader.GetOrdinal("version")) ? null : reader.GetString(reader.GetOrdinal("version")),
            FechaLanzamiento = reader.IsDBNull(reader.GetOrdinal("fecha_lanzamiento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_lanzamiento")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
            });
        }
        return result;
    }

    public async Task<VersionSistemaDto?> ObtenerAsync(int idVersionSistema, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_version_sistema", SqlDbType.Int, idVersionSistema));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new VersionSistemaDto
            {
            IdVersionSistema = reader.GetInt32(reader.GetOrdinal("id_version_sistema")),
            Version = reader.IsDBNull(reader.GetOrdinal("version")) ? null : reader.GetString(reader.GetOrdinal("version")),
            FechaLanzamiento = reader.IsDBNull(reader.GetOrdinal("fecha_lanzamiento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_lanzamiento")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(VersionSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@version", SqlDbType.VarChar, dto.Version, 50));
        command.Parameters.Add(CreateParameter("@fecha_lanzamiento", SqlDbType.DateTime2, dto.FechaLanzamiento));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(VersionSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_version_sistema", SqlDbType.Int, dto.IdVersionSistema));
        command.Parameters.Add(CreateParameter("@version", SqlDbType.VarChar, dto.Version, 50));
        command.Parameters.Add(CreateParameter("@fecha_lanzamiento", SqlDbType.DateTime2, dto.FechaLanzamiento));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(int idVersionSistema, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_version_sistema", SqlDbType.Int, idVersionSistema));
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
