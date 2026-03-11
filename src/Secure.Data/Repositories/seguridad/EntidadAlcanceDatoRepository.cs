using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.entidad_alcance_dato con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EntidadAlcanceDatoRepository : IEntidadAlcanceDatoRepository
{
    private const string SpListar = "seguridad.usp_entidad_alcance_dato_listar";
    private const string SpObtener = "seguridad.usp_entidad_alcance_dato_obtener";
    private const string SpCrear = "seguridad.usp_entidad_alcance_dato_crear";
    private const string SpActualizar = "seguridad.usp_entidad_alcance_dato_actualizar";
    private const string SpDesactivar = "seguridad.usp_entidad_alcance_dato_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public EntidadAlcanceDatoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<EntidadAlcanceDatoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<EntidadAlcanceDatoDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new EntidadAlcanceDatoDto
            {
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            NombreTabla = reader.IsDBNull(reader.GetOrdinal("nombre_tabla")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_tabla")),
            ColumnaLlavePrimaria = reader.IsDBNull(reader.GetOrdinal("columna_llave_primaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("columna_llave_primaria")),
            ColumnaTenant = reader.IsDBNull(reader.GetOrdinal("columna_tenant")) ? null : reader.GetString(reader.GetOrdinal("columna_tenant")),
            ColumnaEmpresa = reader.IsDBNull(reader.GetOrdinal("columna_empresa")) ? null : reader.GetString(reader.GetOrdinal("columna_empresa")),
            ColumnaUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("columna_unidad_organizativa")) ? null : reader.GetString(reader.GetOrdinal("columna_unidad_organizativa")),
            ColumnaPropietario = reader.IsDBNull(reader.GetOrdinal("columna_propietario")) ? null : reader.GetString(reader.GetOrdinal("columna_propietario")),
            ColumnaContexto = reader.IsDBNull(reader.GetOrdinal("columna_contexto")) ? null : reader.GetString(reader.GetOrdinal("columna_contexto")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<EntidadAlcanceDatoDto?> ObtenerAsync(string codigoEntidad, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, codigoEntidad, 128));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new EntidadAlcanceDatoDto
            {
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            NombreTabla = reader.IsDBNull(reader.GetOrdinal("nombre_tabla")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_tabla")),
            ColumnaLlavePrimaria = reader.IsDBNull(reader.GetOrdinal("columna_llave_primaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("columna_llave_primaria")),
            ColumnaTenant = reader.IsDBNull(reader.GetOrdinal("columna_tenant")) ? null : reader.GetString(reader.GetOrdinal("columna_tenant")),
            ColumnaEmpresa = reader.IsDBNull(reader.GetOrdinal("columna_empresa")) ? null : reader.GetString(reader.GetOrdinal("columna_empresa")),
            ColumnaUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("columna_unidad_organizativa")) ? null : reader.GetString(reader.GetOrdinal("columna_unidad_organizativa")),
            ColumnaPropietario = reader.IsDBNull(reader.GetOrdinal("columna_propietario")) ? null : reader.GetString(reader.GetOrdinal("columna_propietario")),
            ColumnaContexto = reader.IsDBNull(reader.GetOrdinal("columna_contexto")) ? null : reader.GetString(reader.GetOrdinal("columna_contexto")),
            Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? null : reader.GetString(reader.GetOrdinal("descripcion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<string> CrearAsync(EntidadAlcanceDatoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@nombre_tabla", SqlDbType.NVarChar, dto.NombreTabla, 128));
        command.Parameters.Add(CreateParameter("@columna_llave_primaria", SqlDbType.NVarChar, dto.ColumnaLlavePrimaria, 128));
        command.Parameters.Add(CreateParameter("@columna_tenant", SqlDbType.NVarChar, dto.ColumnaTenant, 128));
        command.Parameters.Add(CreateParameter("@columna_empresa", SqlDbType.NVarChar, dto.ColumnaEmpresa, 128));
        command.Parameters.Add(CreateParameter("@columna_unidad_organizativa", SqlDbType.NVarChar, dto.ColumnaUnidadOrganizativa, 128));
        command.Parameters.Add(CreateParameter("@columna_propietario", SqlDbType.NVarChar, dto.ColumnaPropietario, 128));
        command.Parameters.Add(CreateParameter("@columna_contexto", SqlDbType.NVarChar, dto.ColumnaContexto, 128));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 256));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return result?.ToString() ?? string.Empty;
    }

    public async Task<bool> ActualizarAsync(EntidadAlcanceDatoDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@nombre_tabla", SqlDbType.NVarChar, dto.NombreTabla, 128));
        command.Parameters.Add(CreateParameter("@columna_llave_primaria", SqlDbType.NVarChar, dto.ColumnaLlavePrimaria, 128));
        command.Parameters.Add(CreateParameter("@columna_tenant", SqlDbType.NVarChar, dto.ColumnaTenant, 128));
        command.Parameters.Add(CreateParameter("@columna_empresa", SqlDbType.NVarChar, dto.ColumnaEmpresa, 128));
        command.Parameters.Add(CreateParameter("@columna_unidad_organizativa", SqlDbType.NVarChar, dto.ColumnaUnidadOrganizativa, 128));
        command.Parameters.Add(CreateParameter("@columna_propietario", SqlDbType.NVarChar, dto.ColumnaPropietario, 128));
        command.Parameters.Add(CreateParameter("@columna_contexto", SqlDbType.NVarChar, dto.ColumnaContexto, 128));
        command.Parameters.Add(CreateParameter("@descripcion", SqlDbType.NVarChar, dto.Descripcion, 256));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(string codigoEntidad, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, codigoEntidad, 128));
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
