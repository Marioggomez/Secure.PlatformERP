using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.contacto_tercero con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ContactoTerceroRepository : IContactoTerceroRepository
{
    private const string SpListar = "tercero.usp_contacto_tercero_listar";
    private const string SpObtener = "tercero.usp_contacto_tercero_obtener";
    private const string SpCrear = "tercero.usp_contacto_tercero_crear";
    private const string SpActualizar = "tercero.usp_contacto_tercero_actualizar";
    private const string SpDesactivar = "tercero.usp_contacto_tercero_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ContactoTerceroRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ContactoTerceroDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ContactoTerceroDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ContactoTerceroDto
            {
            IdContactoTercero = reader.GetInt64(reader.GetOrdinal("id_contacto_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoContacto = reader.GetInt32(reader.GetOrdinal("id_tipo_contacto")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            });
        }
        return result;
    }

    public async Task<ContactoTerceroDto?> ObtenerAsync(long idContactoTercero, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_contacto_tercero", SqlDbType.BigInt, idContactoTercero));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ContactoTerceroDto
            {
            IdContactoTercero = reader.GetInt64(reader.GetOrdinal("id_contacto_tercero")),
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            IdTipoContacto = reader.GetInt32(reader.GetOrdinal("id_tipo_contacto")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor")),
            Principal = reader.GetBoolean(reader.GetOrdinal("principal"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ContactoTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_contacto", SqlDbType.Int, dto.IdTipoContacto));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.NVarChar, dto.Valor, 300));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ContactoTerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_contacto_tercero", SqlDbType.BigInt, dto.IdContactoTercero));
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@id_tipo_contacto", SqlDbType.Int, dto.IdTipoContacto));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.NVarChar, dto.Valor, 300));
        command.Parameters.Add(CreateParameter("@principal", SqlDbType.Bit, dto.Principal));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idContactoTercero, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_contacto_tercero", SqlDbType.BigInt, idContactoTercero));
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
