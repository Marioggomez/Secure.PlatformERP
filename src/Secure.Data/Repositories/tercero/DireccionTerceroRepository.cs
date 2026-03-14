using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.direccion_tercero con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class DireccionTerceroRepository : IDireccionTerceroRepository
{
    private const string SpListar = "tercero.usp_direccion_tercero_listar";
    private const string SpObtener = "tercero.usp_direccion_tercero_obtener";
    private const string SpCrear = "tercero.usp_direccion_tercero_crear";
    private const string SpActualizar = "tercero.usp_direccion_tercero_actualizar";
    private const string SpDesactivar = "tercero.usp_direccion_tercero_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public DireccionTerceroRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<DireccionTerceroDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<DireccionTerceroDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new DireccionTerceroDto
            {
            IdDireccionTercero = reader.GetInt64(reader.GetOrdinal("id_direccion_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoDireccion = reader.GetInt32(reader.GetOrdinal("id_tipo_direccion")),
            DireccionLinea1 = reader.IsDBNull(reader.GetOrdinal("direccion_linea1")) ? null : reader.GetString(reader.GetOrdinal("direccion_linea1")),
            DireccionLinea2 = reader.IsDBNull(reader.GetOrdinal("direccion_linea2")) ? null : reader.GetString(reader.GetOrdinal("direccion_linea2")),
            IdPais = reader.IsDBNull(reader.GetOrdinal("id_pais")) ? null : reader.GetInt32(reader.GetOrdinal("id_pais")),
            IdEstado = reader.IsDBNull(reader.GetOrdinal("id_estado")) ? null : reader.GetInt32(reader.GetOrdinal("id_estado")),
            IdCiudad = reader.IsDBNull(reader.GetOrdinal("id_ciudad")) ? null : reader.GetInt32(reader.GetOrdinal("id_ciudad")),
            CodigoPostal = reader.IsDBNull(reader.GetOrdinal("codigo_postal")) ? null : reader.GetString(reader.GetOrdinal("codigo_postal")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            });
        }
        return result;
    }

    public async Task<DireccionTerceroDto?> ObtenerAsync(long idDireccionTercero, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_direccion_tercero", SqlDbType.BigInt, idDireccionTercero));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new DireccionTerceroDto
            {
            IdDireccionTercero = reader.GetInt64(reader.GetOrdinal("id_direccion_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoDireccion = reader.GetInt32(reader.GetOrdinal("id_tipo_direccion")),
            DireccionLinea1 = reader.IsDBNull(reader.GetOrdinal("direccion_linea1")) ? null : reader.GetString(reader.GetOrdinal("direccion_linea1")),
            DireccionLinea2 = reader.IsDBNull(reader.GetOrdinal("direccion_linea2")) ? null : reader.GetString(reader.GetOrdinal("direccion_linea2")),
            IdPais = reader.IsDBNull(reader.GetOrdinal("id_pais")) ? null : reader.GetInt32(reader.GetOrdinal("id_pais")),
            IdEstado = reader.IsDBNull(reader.GetOrdinal("id_estado")) ? null : reader.GetInt32(reader.GetOrdinal("id_estado")),
            IdCiudad = reader.IsDBNull(reader.GetOrdinal("id_ciudad")) ? null : reader.GetInt32(reader.GetOrdinal("id_ciudad")),
            CodigoPostal = reader.IsDBNull(reader.GetOrdinal("codigo_postal")) ? null : reader.GetString(reader.GetOrdinal("codigo_postal")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(DireccionTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_direccion", SqlDbType.Int, dto.IdTipoDireccion));
        command.Parameters.Add(CreateParameter("@direccion_linea1", SqlDbType.NVarChar, dto.DireccionLinea1, 400));
        command.Parameters.Add(CreateParameter("@direccion_linea2", SqlDbType.NVarChar, dto.DireccionLinea2, 400));
        command.Parameters.Add(CreateParameter("@id_pais", SqlDbType.Int, dto.IdPais));
        command.Parameters.Add(CreateParameter("@id_estado", SqlDbType.Int, dto.IdEstado));
        command.Parameters.Add(CreateParameter("@id_ciudad", SqlDbType.Int, dto.IdCiudad));
        command.Parameters.Add(CreateParameter("@codigo_postal", SqlDbType.NVarChar, dto.CodigoPostal, 50));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(DireccionTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_direccion_tercero", SqlDbType.BigInt, dto.IdDireccionTercero));
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_direccion", SqlDbType.Int, dto.IdTipoDireccion));
        command.Parameters.Add(CreateParameter("@direccion_linea1", SqlDbType.NVarChar, dto.DireccionLinea1, 400));
        command.Parameters.Add(CreateParameter("@direccion_linea2", SqlDbType.NVarChar, dto.DireccionLinea2, 400));
        command.Parameters.Add(CreateParameter("@id_pais", SqlDbType.Int, dto.IdPais));
        command.Parameters.Add(CreateParameter("@id_estado", SqlDbType.Int, dto.IdEstado));
        command.Parameters.Add(CreateParameter("@id_ciudad", SqlDbType.Int, dto.IdCiudad));
        command.Parameters.Add(CreateParameter("@codigo_postal", SqlDbType.NVarChar, dto.CodigoPostal, 50));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return affected is not null && affected != DBNull.Value && Convert.ToInt64(affected) > 0;
    }

    public async Task<bool> DesactivarAsync(long idDireccionTercero, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_direccion_tercero", SqlDbType.BigInt, idDireccionTercero));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return affected is not null && affected != DBNull.Value && Convert.ToInt64(affected) > 0;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}

