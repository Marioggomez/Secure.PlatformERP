using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Catalogo;
using Secure.Platform.Data.Repositories.Interfaces.Catalogo;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Catalogo;

/// <summary>
/// Repositorio ADO.NET para catalogo.tipo_origen_autenticacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TipoOrigenAutenticacionRepository : ITipoOrigenAutenticacionRepository
{
    private const string SpListar = "catalogo.usp_tipo_origen_autenticacion_listar";
    private const string SpObtener = "catalogo.usp_tipo_origen_autenticacion_obtener";
    private const string SpCrear = "catalogo.usp_tipo_origen_autenticacion_crear";
    private const string SpActualizar = "catalogo.usp_tipo_origen_autenticacion_actualizar";
    private const string SpDesactivar = "catalogo.usp_tipo_origen_autenticacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public TipoOrigenAutenticacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<TipoOrigenAutenticacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<TipoOrigenAutenticacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new TipoOrigenAutenticacionDto
            {
            IdTipoOrigenAutenticacion = reader.GetInt16(reader.GetOrdinal("id_tipo_origen_autenticacion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            OrdenVisual = reader.GetInt16(reader.GetOrdinal("orden_visual")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<TipoOrigenAutenticacionDto?> ObtenerAsync(short idTipoOrigenAutenticacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_tipo_origen_autenticacion", SqlDbType.SmallInt, idTipoOrigenAutenticacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new TipoOrigenAutenticacionDto
            {
            IdTipoOrigenAutenticacion = reader.GetInt16(reader.GetOrdinal("id_tipo_origen_autenticacion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            OrdenVisual = reader.GetInt16(reader.GetOrdinal("orden_visual")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<short> CrearAsync(TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 30));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 120));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 300));
        command.Parameters.Add(CreateParameter("@orden_visual", SqlDbType.SmallInt, dto.OrdenVisual));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt16(result);
    }

    public async Task<bool> ActualizarAsync(TipoOrigenAutenticacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_tipo_origen_autenticacion", SqlDbType.SmallInt, dto.IdTipoOrigenAutenticacion));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 30));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 120));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 300));
        command.Parameters.Add(CreateParameter("@orden_visual", SqlDbType.SmallInt, dto.OrdenVisual));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(short idTipoOrigenAutenticacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_tipo_origen_autenticacion", SqlDbType.SmallInt, idTipoOrigenAutenticacion));
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
