using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Organizacion;

/// <summary>
/// Repositorio ADO.NET para organizacion.unidad_organizativa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UnidadOrganizativaRepository : IUnidadOrganizativaRepository
{
    private const string SpListar = "organizacion.usp_unidad_organizativa_listar";
    private const string SpObtener = "organizacion.usp_unidad_organizativa_obtener";
    private const string SpCrear = "organizacion.usp_unidad_organizativa_crear";
    private const string SpActualizar = "organizacion.usp_unidad_organizativa_actualizar";
    private const string SpDesactivar = "organizacion.usp_unidad_organizativa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public UnidadOrganizativaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<UnidadOrganizativaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<UnidadOrganizativaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UnidadOrganizativaDto
            {
            IdUnidadOrganizativa = reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdTipoUnidadOrganizativa = reader.GetInt16(reader.GetOrdinal("id_tipo_unidad_organizativa")),
            IdUnidadPadre = reader.IsDBNull(reader.GetOrdinal("id_unidad_padre")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_padre")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            NivelJerarquia = reader.GetInt16(reader.GetOrdinal("nivel_jerarquia")),
            RutaJerarquia = reader.IsDBNull(reader.GetOrdinal("ruta_jerarquia")) ? null : reader.GetString(reader.GetOrdinal("ruta_jerarquia")),
            EsHoja = reader.GetBoolean(reader.GetOrdinal("es_hoja")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<UnidadOrganizativaDto?> ObtenerAsync(long idUnidadOrganizativa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, idUnidadOrganizativa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new UnidadOrganizativaDto
            {
            IdUnidadOrganizativa = reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdTipoUnidadOrganizativa = reader.GetInt16(reader.GetOrdinal("id_tipo_unidad_organizativa")),
            IdUnidadPadre = reader.IsDBNull(reader.GetOrdinal("id_unidad_padre")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_padre")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
            NivelJerarquia = reader.GetInt16(reader.GetOrdinal("nivel_jerarquia")),
            RutaJerarquia = reader.IsDBNull(reader.GetOrdinal("ruta_jerarquia")) ? null : reader.GetString(reader.GetOrdinal("ruta_jerarquia")),
            EsHoja = reader.GetBoolean(reader.GetOrdinal("es_hoja")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(UnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_tipo_unidad_organizativa", SqlDbType.SmallInt, dto.IdTipoUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_unidad_padre", SqlDbType.BigInt, dto.IdUnidadPadre));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 60));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@nivel_jerarquia", SqlDbType.SmallInt, dto.NivelJerarquia));
        command.Parameters.Add(CreateParameter("@ruta_jerarquia", SqlDbType.NVarChar, dto.RutaJerarquia, 500));
        command.Parameters.Add(CreateParameter("@es_hoja", SqlDbType.Bit, dto.EsHoja));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(UnidadOrganizativaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_tipo_unidad_organizativa", SqlDbType.SmallInt, dto.IdTipoUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_unidad_padre", SqlDbType.BigInt, dto.IdUnidadPadre));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 60));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@nivel_jerarquia", SqlDbType.SmallInt, dto.NivelJerarquia));
        command.Parameters.Add(CreateParameter("@ruta_jerarquia", SqlDbType.NVarChar, dto.RutaJerarquia, 500));
        command.Parameters.Add(CreateParameter("@es_hoja", SqlDbType.Bit, dto.EsHoja));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUnidadOrganizativa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, idUnidadOrganizativa));
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
