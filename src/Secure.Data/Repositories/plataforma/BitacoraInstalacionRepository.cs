using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.bitacora_instalacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class BitacoraInstalacionRepository : IBitacoraInstalacionRepository
{
    private const string SpListar = "plataforma.usp_bitacora_instalacion_listar";
    private const string SpObtener = "plataforma.usp_bitacora_instalacion_obtener";
    private const string SpCrear = "plataforma.usp_bitacora_instalacion_crear";
    private const string SpActualizar = "plataforma.usp_bitacora_instalacion_actualizar";
    private const string SpDesactivar = "plataforma.usp_bitacora_instalacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public BitacoraInstalacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<BitacoraInstalacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<BitacoraInstalacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new BitacoraInstalacionDto
            {
            IdBitacoraInstalacion = reader.GetInt64(reader.GetOrdinal("id_bitacora_instalacion")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("componente")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado")),
            Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString(reader.GetOrdinal("detalle")),
            IniciadoUtc = reader.GetDateTime(reader.GetOrdinal("iniciado_utc")),
            FinalizadoUtc = reader.IsDBNull(reader.GetOrdinal("finalizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("finalizado_utc"))
            });
        }
        return result;
    }

    public async Task<BitacoraInstalacionDto?> ObtenerAsync(long idBitacoraInstalacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_bitacora_instalacion", SqlDbType.BigInt, idBitacoraInstalacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new BitacoraInstalacionDto
            {
            IdBitacoraInstalacion = reader.GetInt64(reader.GetOrdinal("id_bitacora_instalacion")),
            Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("componente")),
            Accion = reader.IsDBNull(reader.GetOrdinal("accion")) ? string.Empty : reader.GetString(reader.GetOrdinal("accion")),
            Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado")),
            Detalle = reader.IsDBNull(reader.GetOrdinal("detalle")) ? null : reader.GetString(reader.GetOrdinal("detalle")),
            IniciadoUtc = reader.GetDateTime(reader.GetOrdinal("iniciado_utc")),
            FinalizadoUtc = reader.IsDBNull(reader.GetOrdinal("finalizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("finalizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(BitacoraInstalacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 100));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 50));
        command.Parameters.Add(CreateParameter("@estado", SqlDbType.NVarChar, dto.Estado, 30));
        command.Parameters.Add(CreateParameter("@detalle", SqlDbType.NVarChar, dto.Detalle, -1));
        command.Parameters.Add(CreateParameter("@iniciado_utc", SqlDbType.DateTime2, dto.IniciadoUtc));
        command.Parameters.Add(CreateParameter("@finalizado_utc", SqlDbType.DateTime2, dto.FinalizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(BitacoraInstalacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_bitacora_instalacion", SqlDbType.BigInt, dto.IdBitacoraInstalacion));
        command.Parameters.Add(CreateParameter("@componente", SqlDbType.NVarChar, dto.Componente, 100));
        command.Parameters.Add(CreateParameter("@accion", SqlDbType.NVarChar, dto.Accion, 50));
        command.Parameters.Add(CreateParameter("@estado", SqlDbType.NVarChar, dto.Estado, 30));
        command.Parameters.Add(CreateParameter("@detalle", SqlDbType.NVarChar, dto.Detalle, -1));
        command.Parameters.Add(CreateParameter("@iniciado_utc", SqlDbType.DateTime2, dto.IniciadoUtc));
        command.Parameters.Add(CreateParameter("@finalizado_utc", SqlDbType.DateTime2, dto.FinalizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idBitacoraInstalacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_bitacora_instalacion", SqlDbType.BigInt, idBitacoraInstalacion));
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
