using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.modulo_permiso con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ModuloPermisoRepository : IModuloPermisoRepository
{
    private const string SpListar = "seguridad.usp_modulo_permiso_listar";
    private const string SpObtener = "seguridad.usp_modulo_permiso_obtener";
    private const string SpCrear = "seguridad.usp_modulo_permiso_crear";
    private const string SpActualizar = "seguridad.usp_modulo_permiso_actualizar";
    private const string SpDesactivar = "seguridad.usp_modulo_permiso_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ModuloPermisoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ModuloPermisoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ModuloPermisoDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ModuloPermisoDto
            {
            IdModuloPermiso = reader.GetInt64(reader.GetOrdinal("id_modulo_permiso")),
            IdModulo = reader.IsDBNull(reader.GetOrdinal("id_modulo")) ? null : reader.GetInt32(reader.GetOrdinal("id_modulo")),
            IdPermiso = reader.IsDBNull(reader.GetOrdinal("id_permiso")) ? null : reader.GetInt64(reader.GetOrdinal("id_permiso"))
            });
        }
        return result;
    }

    public async Task<ModuloPermisoDto?> ObtenerAsync(long idModuloPermiso, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_modulo_permiso", SqlDbType.BigInt, idModuloPermiso));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ModuloPermisoDto
            {
            IdModuloPermiso = reader.GetInt64(reader.GetOrdinal("id_modulo_permiso")),
            IdModulo = reader.IsDBNull(reader.GetOrdinal("id_modulo")) ? null : reader.GetInt32(reader.GetOrdinal("id_modulo")),
            IdPermiso = reader.IsDBNull(reader.GetOrdinal("id_permiso")) ? null : reader.GetInt64(reader.GetOrdinal("id_permiso"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ModuloPermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_modulo", SqlDbType.Int, dto.IdModulo));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.BigInt, dto.IdPermiso));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ModuloPermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_modulo_permiso", SqlDbType.BigInt, dto.IdModuloPermiso));
        command.Parameters.Add(CreateParameter("@id_modulo", SqlDbType.Int, dto.IdModulo));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.BigInt, dto.IdPermiso));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idModuloPermiso, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_modulo_permiso", SqlDbType.BigInt, idModuloPermiso));
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
