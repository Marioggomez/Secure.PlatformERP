using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.usuario_scope_unidad con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioScopeUnidadRepository : IUsuarioScopeUnidadRepository
{
    private const string SpListar = "seguridad.usp_usuario_scope_unidad_listar";
    private const string SpObtener = "seguridad.usp_usuario_scope_unidad_obtener";
    private const string SpCrear = "seguridad.usp_usuario_scope_unidad_crear";
    private const string SpActualizar = "seguridad.usp_usuario_scope_unidad_actualizar";
    private const string SpDesactivar = "seguridad.usp_usuario_scope_unidad_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioScopeUnidadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<UsuarioScopeUnidadDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<UsuarioScopeUnidadDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UsuarioScopeUnidadDto
            {
            IdUsuarioScopeUnidad = reader.GetInt64(reader.GetOrdinal("id_usuario_scope_unidad")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdUnidadOrganizativa = reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa"))
            });
        }
        return result;
    }

    public async Task<UsuarioScopeUnidadDto?> ObtenerAsync(long idUsuarioScopeUnidad, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_unidad", SqlDbType.BigInt, idUsuarioScopeUnidad));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new UsuarioScopeUnidadDto
            {
            IdUsuarioScopeUnidad = reader.GetInt64(reader.GetOrdinal("id_usuario_scope_unidad")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdUnidadOrganizativa = reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(UsuarioScopeUnidadDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(UsuarioScopeUnidadDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_unidad", SqlDbType.BigInt, dto.IdUsuarioScopeUnidad));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUsuarioScopeUnidad, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_unidad", SqlDbType.BigInt, idUsuarioScopeUnidad));
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
