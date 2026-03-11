using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.politica_operacion_api con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaOperacionApiRepository : IPoliticaOperacionApiRepository
{
    private const string SpListar = "seguridad.usp_politica_operacion_api_listar";
    private const string SpObtener = "seguridad.usp_politica_operacion_api_obtener";
    private const string SpCrear = "seguridad.usp_politica_operacion_api_crear";
    private const string SpActualizar = "seguridad.usp_politica_operacion_api_actualizar";
    private const string SpDesactivar = "seguridad.usp_politica_operacion_api_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PoliticaOperacionApiRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PoliticaOperacionApiDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PoliticaOperacionApiDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PoliticaOperacionApiDto
            {
            IdPoliticaOperacionApi = reader.GetInt64(reader.GetOrdinal("id_politica_operacion_api")),
            IdOperacionApi = reader.GetInt64(reader.GetOrdinal("id_operacion_api")),
            IdPermiso = reader.IsDBNull(reader.GetOrdinal("id_permiso")) ? null : reader.GetInt32(reader.GetOrdinal("id_permiso")),
            RequiereAutenticacion = reader.GetBoolean(reader.GetOrdinal("requiere_autenticacion")),
            RequiereSesion = reader.GetBoolean(reader.GetOrdinal("requiere_sesion")),
            RequiereEmpresa = reader.GetBoolean(reader.GetOrdinal("requiere_empresa")),
            RequiereUnidadOrganizativa = reader.GetBoolean(reader.GetOrdinal("requiere_unidad_organizativa")),
            RequiereMfa = reader.GetBoolean(reader.GetOrdinal("requiere_mfa")),
            RequiereAuditoria = reader.GetBoolean(reader.GetOrdinal("requiere_auditoria")),
            RequiereAprobacion = reader.GetBoolean(reader.GetOrdinal("requiere_aprobacion")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<PoliticaOperacionApiDto?> ObtenerAsync(long idPoliticaOperacionApi, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_politica_operacion_api", SqlDbType.BigInt, idPoliticaOperacionApi));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PoliticaOperacionApiDto
            {
            IdPoliticaOperacionApi = reader.GetInt64(reader.GetOrdinal("id_politica_operacion_api")),
            IdOperacionApi = reader.GetInt64(reader.GetOrdinal("id_operacion_api")),
            IdPermiso = reader.IsDBNull(reader.GetOrdinal("id_permiso")) ? null : reader.GetInt32(reader.GetOrdinal("id_permiso")),
            RequiereAutenticacion = reader.GetBoolean(reader.GetOrdinal("requiere_autenticacion")),
            RequiereSesion = reader.GetBoolean(reader.GetOrdinal("requiere_sesion")),
            RequiereEmpresa = reader.GetBoolean(reader.GetOrdinal("requiere_empresa")),
            RequiereUnidadOrganizativa = reader.GetBoolean(reader.GetOrdinal("requiere_unidad_organizativa")),
            RequiereMfa = reader.GetBoolean(reader.GetOrdinal("requiere_mfa")),
            RequiereAuditoria = reader.GetBoolean(reader.GetOrdinal("requiere_auditoria")),
            RequiereAprobacion = reader.GetBoolean(reader.GetOrdinal("requiere_aprobacion")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PoliticaOperacionApiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_operacion_api", SqlDbType.BigInt, dto.IdOperacionApi));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@requiere_autenticacion", SqlDbType.Bit, dto.RequiereAutenticacion));
        command.Parameters.Add(CreateParameter("@requiere_sesion", SqlDbType.Bit, dto.RequiereSesion));
        command.Parameters.Add(CreateParameter("@requiere_empresa", SqlDbType.Bit, dto.RequiereEmpresa));
        command.Parameters.Add(CreateParameter("@requiere_unidad_organizativa", SqlDbType.Bit, dto.RequiereUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@requiere_mfa", SqlDbType.Bit, dto.RequiereMfa));
        command.Parameters.Add(CreateParameter("@requiere_auditoria", SqlDbType.Bit, dto.RequiereAuditoria));
        command.Parameters.Add(CreateParameter("@requiere_aprobacion", SqlDbType.Bit, dto.RequiereAprobacion));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PoliticaOperacionApiDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_politica_operacion_api", SqlDbType.BigInt, dto.IdPoliticaOperacionApi));
        command.Parameters.Add(CreateParameter("@id_operacion_api", SqlDbType.BigInt, dto.IdOperacionApi));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@requiere_autenticacion", SqlDbType.Bit, dto.RequiereAutenticacion));
        command.Parameters.Add(CreateParameter("@requiere_sesion", SqlDbType.Bit, dto.RequiereSesion));
        command.Parameters.Add(CreateParameter("@requiere_empresa", SqlDbType.Bit, dto.RequiereEmpresa));
        command.Parameters.Add(CreateParameter("@requiere_unidad_organizativa", SqlDbType.Bit, dto.RequiereUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@requiere_mfa", SqlDbType.Bit, dto.RequiereMfa));
        command.Parameters.Add(CreateParameter("@requiere_auditoria", SqlDbType.Bit, dto.RequiereAuditoria));
        command.Parameters.Add(CreateParameter("@requiere_aprobacion", SqlDbType.Bit, dto.RequiereAprobacion));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idPoliticaOperacionApi, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_politica_operacion_api", SqlDbType.BigInt, idPoliticaOperacionApi));
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
