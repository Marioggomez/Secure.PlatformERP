using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.integracion_externa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IntegracionExternaRepository : IIntegracionExternaRepository
{
    private const string SpListar = "plataforma.usp_integracion_externa_listar";
    private const string SpObtener = "plataforma.usp_integracion_externa_obtener";
    private const string SpCrear = "plataforma.usp_integracion_externa_crear";
    private const string SpActualizar = "plataforma.usp_integracion_externa_actualizar";
    private const string SpDesactivar = "plataforma.usp_integracion_externa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public IntegracionExternaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<IntegracionExternaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<IntegracionExternaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new IntegracionExternaDto
            {
            IdIntegracion = reader.GetInt64(reader.GetOrdinal("id_integracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }
        return result;
    }

    public async Task<IntegracionExternaDto?> ObtenerAsync(long idIntegracion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_integracion", SqlDbType.BigInt, idIntegracion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new IntegracionExternaDto
            {
            IdIntegracion = reader.GetInt64(reader.GetOrdinal("id_integracion")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            Endpoint = reader.IsDBNull(reader.GetOrdinal("endpoint")) ? null : reader.GetString(reader.GetOrdinal("endpoint")),
            Activo = reader.IsDBNull(reader.GetOrdinal("activo")) ? null : reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(IntegracionExternaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 500));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(IntegracionExternaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_integracion", SqlDbType.BigInt, dto.IdIntegracion));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.VarChar, dto.Codigo, 100));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.VarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@endpoint", SqlDbType.VarChar, dto.Endpoint, 500));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idIntegracion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_integracion", SqlDbType.BigInt, idIntegracion));
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
