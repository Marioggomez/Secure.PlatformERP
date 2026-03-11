using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.instancia_aprobacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class InstanciaAprobacionRepository : IInstanciaAprobacionRepository
{
    private const string SpListar = "cumplimiento.usp_instancia_aprobacion_listar";
    private const string SpObtener = "cumplimiento.usp_instancia_aprobacion_obtener";
    private const string SpCrear = "cumplimiento.usp_instancia_aprobacion_crear";
    private const string SpActualizar = "cumplimiento.usp_instancia_aprobacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_instancia_aprobacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public InstanciaAprobacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<InstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<InstanciaAprobacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new InstanciaAprobacionDto
            {
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            IdObjeto = reader.GetInt64(reader.GetOrdinal("id_objeto")),
            NivelActual = reader.GetByte(reader.GetOrdinal("nivel_actual")),
            IdEstadoAprobacion = reader.GetInt16(reader.GetOrdinal("id_estado_aprobacion")),
            SolicitadoPor = reader.GetInt64(reader.GetOrdinal("solicitado_por")),
            SolicitadoUtc = reader.GetDateTime(reader.GetOrdinal("solicitado_utc")),
            ExpiraUtc = reader.IsDBNull(reader.GetOrdinal("expira_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("expira_utc")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            HashPayload = reader.IsDBNull(reader.GetOrdinal("hash_payload")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("hash_payload")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }
        return result;
    }

    public async Task<InstanciaAprobacionDto?> ObtenerAsync(long idInstanciaAprobacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, idInstanciaAprobacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new InstanciaAprobacionDto
            {
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            IdObjeto = reader.GetInt64(reader.GetOrdinal("id_objeto")),
            NivelActual = reader.GetByte(reader.GetOrdinal("nivel_actual")),
            IdEstadoAprobacion = reader.GetInt16(reader.GetOrdinal("id_estado_aprobacion")),
            SolicitadoPor = reader.GetInt64(reader.GetOrdinal("solicitado_por")),
            SolicitadoUtc = reader.GetDateTime(reader.GetOrdinal("solicitado_utc")),
            ExpiraUtc = reader.IsDBNull(reader.GetOrdinal("expira_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("expira_utc")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            HashPayload = reader.IsDBNull(reader.GetOrdinal("hash_payload")) ? null : (byte[])reader.GetValue(reader.GetOrdinal("hash_payload")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(InstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, dto.IdPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@id_objeto", SqlDbType.BigInt, dto.IdObjeto));
        command.Parameters.Add(CreateParameter("@nivel_actual", SqlDbType.TinyInt, dto.NivelActual));
        command.Parameters.Add(CreateParameter("@id_estado_aprobacion", SqlDbType.SmallInt, dto.IdEstadoAprobacion));
        command.Parameters.Add(CreateParameter("@solicitado_por", SqlDbType.BigInt, dto.SolicitadoPor));
        command.Parameters.Add(CreateParameter("@solicitado_utc", SqlDbType.DateTime2, dto.SolicitadoUtc));
        command.Parameters.Add(CreateParameter("@expira_utc", SqlDbType.DateTime2, dto.ExpiraUtc));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@hash_payload", SqlDbType.Binary, dto.HashPayload, 32));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(InstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, dto.IdInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, dto.IdPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@id_objeto", SqlDbType.BigInt, dto.IdObjeto));
        command.Parameters.Add(CreateParameter("@nivel_actual", SqlDbType.TinyInt, dto.NivelActual));
        command.Parameters.Add(CreateParameter("@id_estado_aprobacion", SqlDbType.SmallInt, dto.IdEstadoAprobacion));
        command.Parameters.Add(CreateParameter("@solicitado_por", SqlDbType.BigInt, dto.SolicitadoPor));
        command.Parameters.Add(CreateParameter("@solicitado_utc", SqlDbType.DateTime2, dto.SolicitadoUtc));
        command.Parameters.Add(CreateParameter("@expira_utc", SqlDbType.DateTime2, dto.ExpiraUtc));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@hash_payload", SqlDbType.Binary, dto.HashPayload, 32));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idInstanciaAprobacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, idInstanciaAprobacion));
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
