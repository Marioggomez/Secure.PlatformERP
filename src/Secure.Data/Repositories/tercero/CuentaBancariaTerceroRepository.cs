using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.cuenta_bancaria_tercero con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CuentaBancariaTerceroRepository : ICuentaBancariaTerceroRepository
{
    private const string SpListar = "tercero.usp_cuenta_bancaria_tercero_listar";
    private const string SpObtener = "tercero.usp_cuenta_bancaria_tercero_obtener";
    private const string SpCrear = "tercero.usp_cuenta_bancaria_tercero_crear";
    private const string SpActualizar = "tercero.usp_cuenta_bancaria_tercero_actualizar";
    private const string SpDesactivar = "tercero.usp_cuenta_bancaria_tercero_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public CuentaBancariaTerceroRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<CuentaBancariaTerceroDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<CuentaBancariaTerceroDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new CuentaBancariaTerceroDto
            {
            IdCuentaBancariaTercero = reader.GetInt64(reader.GetOrdinal("id_cuenta_bancaria_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdBanco = reader.GetInt32(reader.GetOrdinal("id_banco")),
            NumeroCuenta = reader.IsDBNull(reader.GetOrdinal("numero_cuenta")) ? string.Empty : reader.GetString(reader.GetOrdinal("numero_cuenta")),
            IdMoneda = reader.IsDBNull(reader.GetOrdinal("id_moneda")) ? null : reader.GetInt32(reader.GetOrdinal("id_moneda")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            });
        }
        return result;
    }

    public async Task<CuentaBancariaTerceroDto?> ObtenerAsync(long idCuentaBancariaTercero, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_cuenta_bancaria_tercero", SqlDbType.BigInt, idCuentaBancariaTercero));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new CuentaBancariaTerceroDto
            {
            IdCuentaBancariaTercero = reader.GetInt64(reader.GetOrdinal("id_cuenta_bancaria_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdBanco = reader.GetInt32(reader.GetOrdinal("id_banco")),
            NumeroCuenta = reader.IsDBNull(reader.GetOrdinal("numero_cuenta")) ? string.Empty : reader.GetString(reader.GetOrdinal("numero_cuenta")),
            IdMoneda = reader.IsDBNull(reader.GetOrdinal("id_moneda")) ? null : reader.GetInt32(reader.GetOrdinal("id_moneda")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(CuentaBancariaTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_banco", SqlDbType.Int, dto.IdBanco));
        command.Parameters.Add(CreateParameter("@numero_cuenta", SqlDbType.NVarChar, dto.NumeroCuenta, 100));
        command.Parameters.Add(CreateParameter("@id_moneda", SqlDbType.Int, dto.IdMoneda));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(CuentaBancariaTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_cuenta_bancaria_tercero", SqlDbType.BigInt, dto.IdCuentaBancariaTercero));
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_banco", SqlDbType.Int, dto.IdBanco));
        command.Parameters.Add(CreateParameter("@numero_cuenta", SqlDbType.NVarChar, dto.NumeroCuenta, 100));
        command.Parameters.Add(CreateParameter("@id_moneda", SqlDbType.Int, dto.IdMoneda));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idCuentaBancariaTercero, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_cuenta_bancaria_tercero", SqlDbType.BigInt, idCuentaBancariaTercero));
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
