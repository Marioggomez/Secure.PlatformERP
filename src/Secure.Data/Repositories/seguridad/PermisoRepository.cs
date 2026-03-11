using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.permiso con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PermisoRepository : IPermisoRepository
{
    private const string SpListar = "seguridad.usp_permiso_listar";
    private const string SpObtener = "seguridad.usp_permiso_obtener";
    private const string SpCrear = "seguridad.usp_permiso_crear";
    private const string SpActualizar = "seguridad.usp_permiso_actualizar";
    private const string SpDesactivar = "seguridad.usp_permiso_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PermisoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PermisoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PermisoDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PermisoDto
            {
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Modulo = reader.IsDBNull(reader.GetOrdinal("modulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modulo")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            EsSensible = reader.GetBoolean(reader.GetOrdinal("es_sensible")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<PermisoDto?> ObtenerAsync(int idPermiso, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, idPermiso));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PermisoDto
            {
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Modulo = reader.IsDBNull(reader.GetOrdinal("modulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("modulo")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            EsSensible = reader.GetBoolean(reader.GetOrdinal("es_sensible")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<int> CrearAsync(PermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 150));
        command.Parameters.Add(CreateParameter("@modulo", SqlDbType.NVarChar, dto.Modulo, 80));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 80));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 400));
        command.Parameters.Add(CreateParameter("@es_sensible", SqlDbType.Bit, dto.EsSensible));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt32(result);
    }

    public async Task<bool> ActualizarAsync(PermisoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 150));
        command.Parameters.Add(CreateParameter("@modulo", SqlDbType.NVarChar, dto.Modulo, 80));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 80));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 400));
        command.Parameters.Add(CreateParameter("@es_sensible", SqlDbType.Bit, dto.EsSensible));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(int idPermiso, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, idPermiso));
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
