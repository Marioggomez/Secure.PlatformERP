using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.regla_sod con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ReglaSodRepository : IReglaSodRepository
{
    private const string SpListar = "cumplimiento.usp_regla_sod_listar";
    private const string SpObtener = "cumplimiento.usp_regla_sod_obtener";
    private const string SpCrear = "cumplimiento.usp_regla_sod_crear";
    private const string SpActualizar = "cumplimiento.usp_regla_sod_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_regla_sod_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ReglaSodRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ReglaSodDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ReglaSodDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ReglaSodDto
            {
            IdReglaSod = reader.GetInt64(reader.GetOrdinal("id_regla_sod")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdPermisoA = reader.GetInt32(reader.GetOrdinal("id_permiso_a")),
            IdPermisoB = reader.GetInt32(reader.GetOrdinal("id_permiso_b")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            ProhibeMismoUsuario = reader.GetBoolean(reader.GetOrdinal("prohibe_mismo_usuario")),
            ProhibeMismaUnidad = reader.GetBoolean(reader.GetOrdinal("prohibe_misma_unidad")),
            ProhibeMismaSesion = reader.GetBoolean(reader.GetOrdinal("prohibe_misma_sesion")),
            ProhibeMismoDia = reader.GetBoolean(reader.GetOrdinal("prohibe_mismo_dia")),
            IdSeveridadSod = reader.GetInt16(reader.GetOrdinal("id_severidad_sod")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<ReglaSodDto?> ObtenerAsync(long idReglaSod, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_regla_sod", SqlDbType.BigInt, idReglaSod));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ReglaSodDto
            {
            IdReglaSod = reader.GetInt64(reader.GetOrdinal("id_regla_sod")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdPermisoA = reader.GetInt32(reader.GetOrdinal("id_permiso_a")),
            IdPermisoB = reader.GetInt32(reader.GetOrdinal("id_permiso_b")),
            CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? null : reader.GetString(reader.GetOrdinal("codigo_entidad")),
            ProhibeMismoUsuario = reader.GetBoolean(reader.GetOrdinal("prohibe_mismo_usuario")),
            ProhibeMismaUnidad = reader.GetBoolean(reader.GetOrdinal("prohibe_misma_unidad")),
            ProhibeMismaSesion = reader.GetBoolean(reader.GetOrdinal("prohibe_misma_sesion")),
            ProhibeMismoDia = reader.GetBoolean(reader.GetOrdinal("prohibe_mismo_dia")),
            IdSeveridadSod = reader.GetInt16(reader.GetOrdinal("id_severidad_sod")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ReglaSodDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_permiso_a", SqlDbType.Int, dto.IdPermisoA));
        command.Parameters.Add(CreateParameter("@id_permiso_b", SqlDbType.Int, dto.IdPermisoB));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@prohibe_mismo_usuario", SqlDbType.Bit, dto.ProhibeMismoUsuario));
        command.Parameters.Add(CreateParameter("@prohibe_misma_unidad", SqlDbType.Bit, dto.ProhibeMismaUnidad));
        command.Parameters.Add(CreateParameter("@prohibe_misma_sesion", SqlDbType.Bit, dto.ProhibeMismaSesion));
        command.Parameters.Add(CreateParameter("@prohibe_mismo_dia", SqlDbType.Bit, dto.ProhibeMismoDia));
        command.Parameters.Add(CreateParameter("@id_severidad_sod", SqlDbType.SmallInt, dto.IdSeveridadSod));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ReglaSodDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_regla_sod", SqlDbType.BigInt, dto.IdReglaSod));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_permiso_a", SqlDbType.Int, dto.IdPermisoA));
        command.Parameters.Add(CreateParameter("@id_permiso_b", SqlDbType.Int, dto.IdPermisoB));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, dto.CodigoEntidad, 128));
        command.Parameters.Add(CreateParameter("@prohibe_mismo_usuario", SqlDbType.Bit, dto.ProhibeMismoUsuario));
        command.Parameters.Add(CreateParameter("@prohibe_misma_unidad", SqlDbType.Bit, dto.ProhibeMismaUnidad));
        command.Parameters.Add(CreateParameter("@prohibe_misma_sesion", SqlDbType.Bit, dto.ProhibeMismaSesion));
        command.Parameters.Add(CreateParameter("@prohibe_mismo_dia", SqlDbType.Bit, dto.ProhibeMismoDia));
        command.Parameters.Add(CreateParameter("@id_severidad_sod", SqlDbType.SmallInt, dto.IdSeveridadSod));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idReglaSod, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_regla_sod", SqlDbType.BigInt, idReglaSod));
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
