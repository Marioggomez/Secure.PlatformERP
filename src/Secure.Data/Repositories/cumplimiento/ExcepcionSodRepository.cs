using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.excepcion_sod con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ExcepcionSodRepository : IExcepcionSodRepository
{
    private const string SpListar = "cumplimiento.usp_excepcion_sod_listar";
    private const string SpObtener = "cumplimiento.usp_excepcion_sod_obtener";
    private const string SpCrear = "cumplimiento.usp_excepcion_sod_crear";
    private const string SpActualizar = "cumplimiento.usp_excepcion_sod_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_excepcion_sod_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ExcepcionSodRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ExcepcionSodDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ExcepcionSodDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ExcepcionSodDto
            {
            IdExcepcionSod = reader.GetInt64(reader.GetOrdinal("id_excepcion_sod")),
            IdReglaSod = reader.GetInt64(reader.GetOrdinal("id_regla_sod")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            AprobadoPor = reader.IsDBNull(reader.GetOrdinal("aprobado_por")) ? null : reader.GetInt64(reader.GetOrdinal("aprobado_por")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<ExcepcionSodDto?> ObtenerAsync(long idExcepcionSod, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_excepcion_sod", SqlDbType.BigInt, idExcepcionSod));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ExcepcionSodDto
            {
            IdExcepcionSod = reader.GetInt64(reader.GetOrdinal("id_excepcion_sod")),
            IdReglaSod = reader.GetInt64(reader.GetOrdinal("id_regla_sod")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            AprobadoPor = reader.IsDBNull(reader.GetOrdinal("aprobado_por")) ? null : reader.GetInt64(reader.GetOrdinal("aprobado_por")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ExcepcionSodDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_regla_sod", SqlDbType.BigInt, dto.IdReglaSod));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@aprobado_por", SqlDbType.BigInt, dto.AprobadoPor));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ExcepcionSodDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_excepcion_sod", SqlDbType.BigInt, dto.IdExcepcionSod));
        command.Parameters.Add(CreateParameter("@id_regla_sod", SqlDbType.BigInt, dto.IdReglaSod));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@aprobado_por", SqlDbType.BigInt, dto.AprobadoPor));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idExcepcionSod, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_excepcion_sod", SqlDbType.BigInt, idExcepcionSod));
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
