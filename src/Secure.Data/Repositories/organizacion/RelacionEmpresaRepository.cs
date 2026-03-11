using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Organizacion;

/// <summary>
/// Repositorio ADO.NET para organizacion.relacion_empresa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class RelacionEmpresaRepository : IRelacionEmpresaRepository
{
    private const string SpListar = "organizacion.usp_relacion_empresa_listar";
    private const string SpObtener = "organizacion.usp_relacion_empresa_obtener";
    private const string SpCrear = "organizacion.usp_relacion_empresa_crear";
    private const string SpActualizar = "organizacion.usp_relacion_empresa_actualizar";
    private const string SpDesactivar = "organizacion.usp_relacion_empresa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public RelacionEmpresaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<RelacionEmpresaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<RelacionEmpresaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new RelacionEmpresaDto
            {
            IdRelacionEmpresa = reader.GetInt64(reader.GetOrdinal("id_relacion_empresa")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresaOrigen = reader.GetInt64(reader.GetOrdinal("id_empresa_origen")),
            IdEmpresaDestino = reader.GetInt64(reader.GetOrdinal("id_empresa_destino")),
            IdTipoRelacionEmpresa = reader.GetInt16(reader.GetOrdinal("id_tipo_relacion_empresa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            Observacion = reader.IsDBNull(reader.GetOrdinal("observacion")) ? null : reader.GetString(reader.GetOrdinal("observacion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<RelacionEmpresaDto?> ObtenerAsync(long idRelacionEmpresa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_relacion_empresa", SqlDbType.BigInt, idRelacionEmpresa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new RelacionEmpresaDto
            {
            IdRelacionEmpresa = reader.GetInt64(reader.GetOrdinal("id_relacion_empresa")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresaOrigen = reader.GetInt64(reader.GetOrdinal("id_empresa_origen")),
            IdEmpresaDestino = reader.GetInt64(reader.GetOrdinal("id_empresa_destino")),
            IdTipoRelacionEmpresa = reader.GetInt16(reader.GetOrdinal("id_tipo_relacion_empresa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            Observacion = reader.IsDBNull(reader.GetOrdinal("observacion")) ? null : reader.GetString(reader.GetOrdinal("observacion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(RelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa_origen", SqlDbType.BigInt, dto.IdEmpresaOrigen));
        command.Parameters.Add(CreateParameter("@id_empresa_destino", SqlDbType.BigInt, dto.IdEmpresaDestino));
        command.Parameters.Add(CreateParameter("@id_tipo_relacion_empresa", SqlDbType.SmallInt, dto.IdTipoRelacionEmpresa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@observacion", SqlDbType.NVarChar, dto.Observacion, 500));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(RelacionEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_relacion_empresa", SqlDbType.BigInt, dto.IdRelacionEmpresa));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa_origen", SqlDbType.BigInt, dto.IdEmpresaOrigen));
        command.Parameters.Add(CreateParameter("@id_empresa_destino", SqlDbType.BigInt, dto.IdEmpresaDestino));
        command.Parameters.Add(CreateParameter("@id_tipo_relacion_empresa", SqlDbType.SmallInt, dto.IdTipoRelacionEmpresa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@observacion", SqlDbType.NVarChar, dto.Observacion, 500));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idRelacionEmpresa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_relacion_empresa", SqlDbType.BigInt, idRelacionEmpresa));
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
