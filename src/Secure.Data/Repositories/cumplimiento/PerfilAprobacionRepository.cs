using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.perfil_aprobacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PerfilAprobacionRepository : IPerfilAprobacionRepository
{
    private const string SpListar = "cumplimiento.usp_perfil_aprobacion_listar";
    private const string SpObtener = "cumplimiento.usp_perfil_aprobacion_obtener";
    private const string SpCrear = "cumplimiento.usp_perfil_aprobacion_crear";
    private const string SpActualizar = "cumplimiento.usp_perfil_aprobacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_perfil_aprobacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PerfilAprobacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PerfilAprobacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PerfilAprobacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PerfilAprobacionDto
            {
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            TipoProceso = reader.IsDBNull(reader.GetOrdinal("tipo_proceso")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_proceso")),
            RequiereMfa = reader.GetBoolean(reader.GetOrdinal("requiere_mfa")),
            ImpideAutoaprobacion = reader.GetBoolean(reader.GetOrdinal("impide_autoaprobacion")),
            ImpideMismaUnidad = reader.GetBoolean(reader.GetOrdinal("impide_misma_unidad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<PerfilAprobacionDto?> ObtenerAsync(long idPerfilAprobacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, idPerfilAprobacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PerfilAprobacionDto
            {
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            TipoProceso = reader.IsDBNull(reader.GetOrdinal("tipo_proceso")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_proceso")),
            RequiereMfa = reader.GetBoolean(reader.GetOrdinal("requiere_mfa")),
            ImpideAutoaprobacion = reader.GetBoolean(reader.GetOrdinal("impide_autoaprobacion")),
            ImpideMismaUnidad = reader.GetBoolean(reader.GetOrdinal("impide_misma_unidad")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 80));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@tipo_proceso", SqlDbType.VarChar, dto.TipoProceso, 40));
        command.Parameters.Add(CreateParameter("@requiere_mfa", SqlDbType.Bit, dto.RequiereMfa));
        command.Parameters.Add(CreateParameter("@impide_autoaprobacion", SqlDbType.Bit, dto.ImpideAutoaprobacion));
        command.Parameters.Add(CreateParameter("@impide_misma_unidad", SqlDbType.Bit, dto.ImpideMismaUnidad));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, dto.IdPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 80));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@tipo_proceso", SqlDbType.VarChar, dto.TipoProceso, 40));
        command.Parameters.Add(CreateParameter("@requiere_mfa", SqlDbType.Bit, dto.RequiereMfa));
        command.Parameters.Add(CreateParameter("@impide_autoaprobacion", SqlDbType.Bit, dto.ImpideAutoaprobacion));
        command.Parameters.Add(CreateParameter("@impide_misma_unidad", SqlDbType.Bit, dto.ImpideMismaUnidad));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idPerfilAprobacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, idPerfilAprobacion));
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
