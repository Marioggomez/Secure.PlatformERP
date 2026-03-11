using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.categoria_configuracion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CategoriaConfiguracionRepository : ICategoriaConfiguracionRepository
{
    private const string SpListar = "plataforma.usp_categoria_configuracion_listar";
    private const string SpObtener = "plataforma.usp_categoria_configuracion_obtener";
    private const string SpCrear = "plataforma.usp_categoria_configuracion_crear";
    private const string SpActualizar = "plataforma.usp_categoria_configuracion_actualizar";
    private const string SpDesactivar = "plataforma.usp_categoria_configuracion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public CategoriaConfiguracionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<CategoriaConfiguracionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<CategoriaConfiguracionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new CategoriaConfiguracionDto
            {
            IdCategoriaConfiguracion = reader.GetInt32(reader.GetOrdinal("id_categoria_configuracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
            });
        }
        return result;
    }

    public async Task<CategoriaConfiguracionDto?> ObtenerAsync(int idCategoriaConfiguracion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_categoria_configuracion", SqlDbType.Int, idCategoriaConfiguracion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new CategoriaConfiguracionDto
            {
            IdCategoriaConfiguracion = reader.GetInt32(reader.GetOrdinal("id_categoria_configuracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(CategoriaConfiguracionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(CategoriaConfiguracionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_categoria_configuracion", SqlDbType.Int, dto.IdCategoriaConfiguracion));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(int idCategoriaConfiguracion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_categoria_configuracion", SqlDbType.Int, idCategoriaConfiguracion));
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
