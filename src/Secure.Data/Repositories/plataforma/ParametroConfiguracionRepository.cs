using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.parametro_configuracion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ParametroConfiguracionRepository : IParametroConfiguracionRepository
{
    private const string SpListar = "plataforma.usp_parametro_configuracion_listar";
    private const string SpObtener = "plataforma.usp_parametro_configuracion_obtener";
    private const string SpCrear = "plataforma.usp_parametro_configuracion_crear";
    private const string SpActualizar = "plataforma.usp_parametro_configuracion_actualizar";
    private const string SpDesactivar = "plataforma.usp_parametro_configuracion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ParametroConfiguracionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ParametroConfiguracionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ParametroConfiguracionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ParametroConfiguracionDto
            {
            IdParametroConfiguracion = reader.GetInt32(reader.GetOrdinal("id_parametro_configuracion")),
            IdCategoriaConfiguracion = reader.GetInt32(reader.GetOrdinal("id_categoria_configuracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            TipoValor = reader.IsDBNull(reader.GetOrdinal("tipo_valor")) ? null : reader.GetString(reader.GetOrdinal("tipo_valor")),
            ValorDefecto = reader.IsDBNull(reader.GetOrdinal("valor_defecto")) ? null : reader.GetString(reader.GetOrdinal("valor_defecto"))
            });
        }
        return result;
    }

    public async Task<ParametroConfiguracionDto?> ObtenerAsync(int idParametroConfiguracion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_parametro_configuracion", SqlDbType.Int, idParametroConfiguracion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ParametroConfiguracionDto
            {
            IdParametroConfiguracion = reader.GetInt32(reader.GetOrdinal("id_parametro_configuracion")),
            IdCategoriaConfiguracion = reader.GetInt32(reader.GetOrdinal("id_categoria_configuracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            TipoValor = reader.IsDBNull(reader.GetOrdinal("tipo_valor")) ? null : reader.GetString(reader.GetOrdinal("tipo_valor")),
            ValorDefecto = reader.IsDBNull(reader.GetOrdinal("valor_defecto")) ? null : reader.GetString(reader.GetOrdinal("valor_defecto"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(ParametroConfiguracionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_categoria_configuracion", SqlDbType.Int, dto.IdCategoriaConfiguracion));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        command.Parameters.Add(CreateParameter("@tipo_valor", SqlDbType.VarChar, dto.TipoValor, 50));
        command.Parameters.Add(CreateParameter("@valor_defecto", SqlDbType.VarChar, dto.ValorDefecto, 500));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(ParametroConfiguracionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_parametro_configuracion", SqlDbType.Int, dto.IdParametroConfiguracion));
        command.Parameters.Add(CreateParameter("@id_categoria_configuracion", SqlDbType.Int, dto.IdCategoriaConfiguracion));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.VarChar, dto.Descripcion, 500));
        command.Parameters.Add(CreateParameter("@tipo_valor", SqlDbType.VarChar, dto.TipoValor, 50));
        command.Parameters.Add(CreateParameter("@valor_defecto", SqlDbType.VarChar, dto.ValorDefecto, 500));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(int idParametroConfiguracion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_parametro_configuracion", SqlDbType.Int, idParametroConfiguracion));
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
