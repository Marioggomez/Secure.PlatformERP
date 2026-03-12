using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.identificacion_tercero con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IdentificacionTerceroRepository : IIdentificacionTerceroRepository
{
    private const string SpListar = "tercero.usp_identificacion_tercero_listar";
    private const string SpObtener = "tercero.usp_identificacion_tercero_obtener";
    private const string SpCrear = "tercero.usp_identificacion_tercero_crear";
    private const string SpActualizar = "tercero.usp_identificacion_tercero_actualizar";
    private const string SpDesactivar = "tercero.usp_identificacion_tercero_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public IdentificacionTerceroRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<IdentificacionTerceroDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<IdentificacionTerceroDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new IdentificacionTerceroDto
            {
            IdIdentificacionTercero = reader.GetInt64(reader.GetOrdinal("id_identificacion_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoIdentificacion = reader.GetInt32(reader.GetOrdinal("id_tipo_identificacion")),
            NumeroIdentificacion = reader.IsDBNull(reader.GetOrdinal("numero_identificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("numero_identificacion")),
            FechaEmision = reader.IsDBNull(reader.GetOrdinal("fecha_emision")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_emision")),
            FechaVencimiento = reader.IsDBNull(reader.GetOrdinal("fecha_vencimiento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            });
        }
        return result;
    }

    public async Task<IdentificacionTerceroDto?> ObtenerAsync(long idIdentificacionTercero, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_identificacion_tercero", SqlDbType.BigInt, idIdentificacionTercero));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new IdentificacionTerceroDto
            {
            IdIdentificacionTercero = reader.GetInt64(reader.GetOrdinal("id_identificacion_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoIdentificacion = reader.GetInt32(reader.GetOrdinal("id_tipo_identificacion")),
            NumeroIdentificacion = reader.IsDBNull(reader.GetOrdinal("numero_identificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("numero_identificacion")),
            FechaEmision = reader.IsDBNull(reader.GetOrdinal("fecha_emision")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_emision")),
            FechaVencimiento = reader.IsDBNull(reader.GetOrdinal("fecha_vencimiento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(IdentificacionTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_identificacion", SqlDbType.Int, dto.IdTipoIdentificacion));
        command.Parameters.Add(CreateParameter("@numero_identificacion", SqlDbType.NVarChar, dto.NumeroIdentificacion, 100));
        command.Parameters.Add(CreateParameter("@fecha_emision", SqlDbType.Date, dto.FechaEmision));
        command.Parameters.Add(CreateParameter("@fecha_vencimiento", SqlDbType.Date, dto.FechaVencimiento));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(IdentificacionTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_identificacion_tercero", SqlDbType.BigInt, dto.IdIdentificacionTercero));
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_identificacion", SqlDbType.Int, dto.IdTipoIdentificacion));
        command.Parameters.Add(CreateParameter("@numero_identificacion", SqlDbType.NVarChar, dto.NumeroIdentificacion, 100));
        command.Parameters.Add(CreateParameter("@fecha_emision", SqlDbType.Date, dto.FechaEmision));
        command.Parameters.Add(CreateParameter("@fecha_vencimiento", SqlDbType.Date, dto.FechaVencimiento));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idIdentificacionTercero, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_identificacion_tercero", SqlDbType.BigInt, idIdentificacionTercero));
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
