using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.job_sistema_ejecucion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class JobSistemaEjecucionRepository : IJobSistemaEjecucionRepository
{
    private const string SpListar = "plataforma.usp_job_sistema_ejecucion_listar";
    private const string SpObtener = "plataforma.usp_job_sistema_ejecucion_obtener";
    private const string SpCrear = "plataforma.usp_job_sistema_ejecucion_crear";
    private const string SpActualizar = "plataforma.usp_job_sistema_ejecucion_actualizar";
    private const string SpDesactivar = "plataforma.usp_job_sistema_ejecucion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public JobSistemaEjecucionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<JobSistemaEjecucionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<JobSistemaEjecucionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new JobSistemaEjecucionDto
            {
            IdEjecucion = reader.GetInt64(reader.GetOrdinal("id_ejecucion")),
            IdJob = reader.IsDBNull(reader.GetOrdinal("id_job")) ? null : reader.GetInt64(reader.GetOrdinal("id_job")),
            FechaInicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
            FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin")),
            Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? null : reader.GetString(reader.GetOrdinal("estado")),
            Mensaje = reader.IsDBNull(reader.GetOrdinal("mensaje")) ? null : reader.GetString(reader.GetOrdinal("mensaje"))
            });
        }
        return result;
    }

    public async Task<JobSistemaEjecucionDto?> ObtenerAsync(long idEjecucion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_ejecucion", SqlDbType.BigInt, idEjecucion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new JobSistemaEjecucionDto
            {
            IdEjecucion = reader.GetInt64(reader.GetOrdinal("id_ejecucion")),
            IdJob = reader.IsDBNull(reader.GetOrdinal("id_job")) ? null : reader.GetInt64(reader.GetOrdinal("id_job")),
            FechaInicio = reader.IsDBNull(reader.GetOrdinal("fecha_inicio")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
            FechaFin = reader.IsDBNull(reader.GetOrdinal("fecha_fin")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin")),
            Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? null : reader.GetString(reader.GetOrdinal("estado")),
            Mensaje = reader.IsDBNull(reader.GetOrdinal("mensaje")) ? null : reader.GetString(reader.GetOrdinal("mensaje"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(JobSistemaEjecucionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_job", SqlDbType.BigInt, dto.IdJob));
        command.Parameters.Add(CreateParameter("@fecha_inicio", SqlDbType.DateTime2, dto.FechaInicio));
        command.Parameters.Add(CreateParameter("@fecha_fin", SqlDbType.DateTime2, dto.FechaFin));
        command.Parameters.Add(CreateParameter("@estado", SqlDbType.VarChar, dto.Estado, 50));
        command.Parameters.Add(CreateParameter("@mensaje", SqlDbType.NVarChar, dto.Mensaje, -1));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(JobSistemaEjecucionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_ejecucion", SqlDbType.BigInt, dto.IdEjecucion));
        command.Parameters.Add(CreateParameter("@id_job", SqlDbType.BigInt, dto.IdJob));
        command.Parameters.Add(CreateParameter("@fecha_inicio", SqlDbType.DateTime2, dto.FechaInicio));
        command.Parameters.Add(CreateParameter("@fecha_fin", SqlDbType.DateTime2, dto.FechaFin));
        command.Parameters.Add(CreateParameter("@estado", SqlDbType.VarChar, dto.Estado, 50));
        command.Parameters.Add(CreateParameter("@mensaje", SqlDbType.NVarChar, dto.Mensaje, -1));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idEjecucion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_ejecucion", SqlDbType.BigInt, idEjecucion));
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
