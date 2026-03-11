using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.recurso_ui con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RecursoUiRepository : IRecursoUiRepository
{
    private const string SpListar = "seguridad.usp_recurso_ui_listar";
    private const string SpObtener = "seguridad.usp_recurso_ui_obtener";
    private const string SpCrear = "seguridad.usp_recurso_ui_crear";
    private const string SpActualizar = "seguridad.usp_recurso_ui_actualizar";
    private const string SpDesactivar = "seguridad.usp_recurso_ui_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public RecursoUiRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<RecursoUiDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<RecursoUiDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new RecursoUiDto
            {
            IdRecursoUi = reader.GetInt64(reader.GetOrdinal("id_recurso_ui")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            IdTipoRecursoUi = reader.GetInt16(reader.GetOrdinal("id_tipo_recurso_ui")),
            Ruta = reader.IsDBNull(reader.GetOrdinal("ruta")) ? null : reader.GetString(reader.GetOrdinal("ruta")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? null : reader.GetString(reader.GetOrdinal("componente")),
            Icono = reader.IsDBNull(reader.GetOrdinal("icono")) ? null : reader.GetString(reader.GetOrdinal("icono")),
            IdRecursoUiPadre = reader.IsDBNull(reader.GetOrdinal("id_recurso_ui_padre")) ? null : reader.GetInt64(reader.GetOrdinal("id_recurso_ui_padre")),
            OrdenVisual = reader.GetInt32(reader.GetOrdinal("orden_visual")),
            EsVisible = reader.GetBoolean(reader.GetOrdinal("es_visible")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<RecursoUiDto?> ObtenerAsync(long idRecursoUi, CancellationToken cancellationToken)
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
            return new RecursoUiDto
            {
            IdRecursoUi = reader.GetInt64(reader.GetOrdinal("id_recurso_ui")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            IdTipoRecursoUi = reader.GetInt16(reader.GetOrdinal("id_tipo_recurso_ui")),
            Ruta = reader.IsDBNull(reader.GetOrdinal("ruta")) ? null : reader.GetString(reader.GetOrdinal("ruta")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? null : reader.GetString(reader.GetOrdinal("componente")),
            Icono = reader.IsDBNull(reader.GetOrdinal("icono")) ? null : reader.GetString(reader.GetOrdinal("icono")),
            IdRecursoUiPadre = reader.IsDBNull(reader.GetOrdinal("id_recurso_ui_padre")) ? null : reader.GetInt64(reader.GetOrdinal("id_recurso_ui_padre")),
            OrdenVisual = reader.GetInt32(reader.GetOrdinal("orden_visual")),
            EsVisible = reader.GetBoolean(reader.GetOrdinal("es_visible")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(RecursoUiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 120));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@id_tipo_recurso_ui", SqlDbType.SmallInt, dto.IdTipoRecursoUi));
        command.Parameters.Add(CreateParameter("@ruta", SqlDbType.NVarChar, dto.Ruta, 300));
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 200));
        command.Parameters.Add(CreateParameter("@icono", SqlDbType.NVarChar, dto.Icono, 100));
        command.Parameters.Add(CreateParameter("@id_recurso_ui_padre", SqlDbType.BigInt, dto.IdRecursoUiPadre));
        command.Parameters.Add(CreateParameter("@orden_visual", SqlDbType.Int, dto.OrdenVisual));
        command.Parameters.Add(CreateParameter("@es_visible", SqlDbType.Bit, dto.EsVisible));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(RecursoUiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_recurso_ui", SqlDbType.BigInt, dto.IdRecursoUi));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 120));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@id_tipo_recurso_ui", SqlDbType.SmallInt, dto.IdTipoRecursoUi));
        command.Parameters.Add(CreateParameter("@ruta", SqlDbType.NVarChar, dto.Ruta, 300));
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 200));
        command.Parameters.Add(CreateParameter("@icono", SqlDbType.NVarChar, dto.Icono, 100));
        command.Parameters.Add(CreateParameter("@id_recurso_ui_padre", SqlDbType.BigInt, dto.IdRecursoUiPadre));
        command.Parameters.Add(CreateParameter("@orden_visual", SqlDbType.Int, dto.OrdenVisual));
        command.Parameters.Add(CreateParameter("@es_visible", SqlDbType.Bit, dto.EsVisible));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
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
