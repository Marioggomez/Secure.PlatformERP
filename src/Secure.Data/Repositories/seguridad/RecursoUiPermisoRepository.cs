using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.recurso_ui_permiso con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiPermisoRepository : IRecursoUiPermisoRepository
{
    private const string SpListar = "seguridad.usp_recurso_ui_permiso_listar";
    private const string SpObtener = "seguridad.usp_recurso_ui_permiso_obtener";
    private const string SpCrear = "seguridad.usp_recurso_ui_permiso_crear";
    private const string SpActualizar = "seguridad.usp_recurso_ui_permiso_actualizar";
    private const string SpDesactivar = "seguridad.usp_recurso_ui_permiso_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public RecursoUiPermisoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<RecursoUiPermisoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<RecursoUiPermisoDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new RecursoUiPermisoDto
            {
            IdRecursoUi = reader.GetInt64(reader.GetOrdinal("id_recurso_ui")),
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<RecursoUiPermisoDto?> ObtenerAsync(long idRecursoUi, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_recurso_ui", SqlDbType.BigInt, idRecursoUi));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new RecursoUiPermisoDto
            {
            IdRecursoUi = reader.GetInt64(reader.GetOrdinal("id_recurso_ui")),
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(RecursoUiPermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_recurso_ui", SqlDbType.BigInt, dto.IdRecursoUi));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(RecursoUiPermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_recurso_ui", SqlDbType.BigInt, dto.IdRecursoUi));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idRecursoUi, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_recurso_ui", SqlDbType.BigInt, idRecursoUi));
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
