using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.job_sistema con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class JobSistemaRepository : IJobSistemaRepository
{
    private const string SpListar = "plataforma.usp_job_sistema_listar";
    private const string SpObtener = "plataforma.usp_job_sistema_obtener";
    private const string SpCrear = "plataforma.usp_job_sistema_crear";
    private const string SpActualizar = "plataforma.usp_job_sistema_actualizar";
    private const string SpDesactivar = "plataforma.usp_job_sistema_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public JobSistemaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<JobSistemaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<JobSistemaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new JobSistemaDto
            {
            IdJob = reader.GetInt64(reader.GetOrdinal("id_job")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Cron = reader.IsDBNull(reader.GetOrdinal("cron")) ? null : reader.GetString(reader.GetOrdinal("cron")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }
        return result;
    }

    public async Task<JobSistemaDto?> ObtenerAsync(long idJob, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_job", SqlDbType.BigInt, idJob));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new JobSistemaDto
            {
            IdJob = reader.GetInt64(reader.GetOrdinal("id_job")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Cron = reader.IsDBNull(reader.GetOrdinal("cron")) ? null : reader.GetString(reader.GetOrdinal("cron")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(JobSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@cron", SqlDbType.VarChar, dto.Cron, 50));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(JobSistemaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_job", SqlDbType.BigInt, dto.IdJob));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@cron", SqlDbType.VarChar, dto.Cron, 50));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idJob, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_job", SqlDbType.BigInt, idJob));
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
