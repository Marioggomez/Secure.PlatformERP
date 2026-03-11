using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.modulo con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ModuloRepository : IModuloRepository
{
    private const string SpListar = "plataforma.usp_modulo_listar";
    private const string SpObtener = "plataforma.usp_modulo_obtener";
    private const string SpCrear = "plataforma.usp_modulo_crear";
    private const string SpActualizar = "plataforma.usp_modulo_actualizar";
    private const string SpDesactivar = "plataforma.usp_modulo_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ModuloRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ModuloDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ModuloDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ModuloDto
            {
            IdModulo = reader.GetInt32(reader.GetOrdinal("id_modulo")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Orden = reader.IsDBNull(reader.GetOrdinal("orden")) ? null : reader.GetInt32(reader.GetOrdinal("orden"))
            });
        }
        return result;
    }

    public async Task<ModuloDto?> ObtenerAsync(int idModulo, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_modulo", SqlDbType.Int, idModulo));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ModuloDto
            {
            IdModulo = reader.GetInt32(reader.GetOrdinal("id_modulo")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Orden = reader.IsDBNull(reader.GetOrdinal("orden")) ? null : reader.GetInt32(reader.GetOrdinal("orden"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(ModuloDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 100));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 200));
        command.Parameters.Add(CreateParameter("@orden", SqlDbType.Int, dto.Orden));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(ModuloDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_modulo", SqlDbType.Int, dto.IdModulo));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 100));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 200));
        command.Parameters.Add(CreateParameter("@orden", SqlDbType.Int, dto.Orden));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(int idModulo, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_modulo", SqlDbType.Int, idModulo));
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
