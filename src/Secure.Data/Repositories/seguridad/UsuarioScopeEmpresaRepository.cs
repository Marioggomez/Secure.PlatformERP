using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.usuario_scope_empresa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioScopeEmpresaRepository : IUsuarioScopeEmpresaRepository
{
    private const string SpListar = "seguridad.usp_usuario_scope_empresa_listar";
    private const string SpObtener = "seguridad.usp_usuario_scope_empresa_obtener";
    private const string SpCrear = "seguridad.usp_usuario_scope_empresa_crear";
    private const string SpActualizar = "seguridad.usp_usuario_scope_empresa_actualizar";
    private const string SpDesactivar = "seguridad.usp_usuario_scope_empresa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioScopeEmpresaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<UsuarioScopeEmpresaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<UsuarioScopeEmpresaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UsuarioScopeEmpresaDto
            {
            IdUsuarioScopeEmpresa = reader.GetInt64(reader.GetOrdinal("id_usuario_scope_empresa")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa"))
            });
        }
        return result;
    }

    public async Task<UsuarioScopeEmpresaDto?> ObtenerAsync(long idUsuarioScopeEmpresa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_empresa", SqlDbType.BigInt, idUsuarioScopeEmpresa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new UsuarioScopeEmpresaDto
            {
            IdUsuarioScopeEmpresa = reader.GetInt64(reader.GetOrdinal("id_usuario_scope_empresa")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(UsuarioScopeEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_empresa", SqlDbType.BigInt, dto.IdUsuarioScopeEmpresa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUsuarioScopeEmpresa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_usuario_scope_empresa", SqlDbType.BigInt, idUsuarioScopeEmpresa));
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
