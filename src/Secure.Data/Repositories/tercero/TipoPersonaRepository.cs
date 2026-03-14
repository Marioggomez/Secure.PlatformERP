using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.tipo_persona con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TipoPersonaRepository : ITipoPersonaRepository
{
    private const string SpListar = "tercero.usp_tipo_persona_listar";
    private const string SpObtener = "tercero.usp_tipo_persona_obtener";
    private const string SpCrear = "tercero.usp_tipo_persona_crear";
    private const string SpActualizar = "tercero.usp_tipo_persona_actualizar";
    private const string SpDesactivar = "tercero.usp_tipo_persona_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public TipoPersonaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<TipoPersonaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<TipoPersonaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new TipoPersonaDto
            {
            IdTipoPersona = reader.GetInt32(reader.GetOrdinal("id_tipo_persona")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre"))
            });
        }
        return result;
    }

    public async Task<TipoPersonaDto?> ObtenerAsync(int idTipoPersona, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_tipo_persona", SqlDbType.Int, idTipoPersona));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new TipoPersonaDto
            {
            IdTipoPersona = reader.GetInt32(reader.GetOrdinal("id_tipo_persona")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(TipoPersonaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 100));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(TipoPersonaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_tipo_persona", SqlDbType.Int, dto.IdTipoPersona));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 100));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return affected is not null && affected != DBNull.Value && Convert.ToInt64(affected) > 0;
    }

    public async Task<bool> DesactivarAsync(int idTipoPersona, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_tipo_persona", SqlDbType.Int, idTipoPersona));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return affected is not null && affected != DBNull.Value && Convert.ToInt64(affected) > 0;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}

