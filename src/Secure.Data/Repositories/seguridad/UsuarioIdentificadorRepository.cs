using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.usuario_identificador con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioIdentificadorRepository : IUsuarioIdentificadorRepository
{
    private const string SpListar = "seguridad.usp_usuario_identificador_listar";
    private const string SpObtener = "seguridad.usp_usuario_identificador_obtener";
    private const string SpCrear = "seguridad.usp_usuario_identificador_crear";
    private const string SpActualizar = "seguridad.usp_usuario_identificador_actualizar";
    private const string SpDesactivar = "seguridad.usp_usuario_identificador_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioIdentificadorRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<UsuarioIdentificadorDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<UsuarioIdentificadorDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UsuarioIdentificadorDto
            {
            IdUsuarioIdentificador = reader.GetInt64(reader.GetOrdinal("id_usuario_identificador")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoIdentificadorUsuario = reader.GetInt16(reader.GetOrdinal("id_tipo_identificador_usuario")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor")),
            ValorNormalizado = reader.IsDBNull(reader.GetOrdinal("valor_normalizado")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor_normalizado")),
            EsPrincipal = reader.GetBoolean(reader.GetOrdinal("es_principal")),
            Verificado = reader.GetBoolean(reader.GetOrdinal("verificado")),
            FechaVerificacionUtc = reader.IsDBNull(reader.GetOrdinal("fecha_verificacion_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_verificacion_utc")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<UsuarioIdentificadorDto?> ObtenerAsync(long idUsuarioIdentificador, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario_identificador", SqlDbType.BigInt, idUsuarioIdentificador));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new UsuarioIdentificadorDto
            {
            IdUsuarioIdentificador = reader.GetInt64(reader.GetOrdinal("id_usuario_identificador")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTipoIdentificadorUsuario = reader.GetInt16(reader.GetOrdinal("id_tipo_identificador_usuario")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor")),
            ValorNormalizado = reader.IsDBNull(reader.GetOrdinal("valor_normalizado")) ? string.Empty : reader.GetString(reader.GetOrdinal("valor_normalizado")),
            EsPrincipal = reader.GetBoolean(reader.GetOrdinal("es_principal")),
            Verificado = reader.GetBoolean(reader.GetOrdinal("verificado")),
            FechaVerificacionUtc = reader.IsDBNull(reader.GetOrdinal("fecha_verificacion_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_verificacion_utc")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(UsuarioIdentificadorDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_identificador_usuario", SqlDbType.SmallInt, dto.IdTipoIdentificadorUsuario));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.NVarChar, dto.Valor, 250));
        command.Parameters.Add(CreateParameter("@valor_normalizado", SqlDbType.NVarChar, dto.ValorNormalizado, 250));
        command.Parameters.Add(CreateParameter("@es_principal", SqlDbType.Bit, dto.EsPrincipal));
        command.Parameters.Add(CreateParameter("@verificado", SqlDbType.Bit, dto.Verificado));
        command.Parameters.Add(CreateParameter("@fecha_verificacion_utc", SqlDbType.DateTime2, dto.FechaVerificacionUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(UsuarioIdentificadorDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_usuario_identificador", SqlDbType.BigInt, dto.IdUsuarioIdentificador));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_identificador_usuario", SqlDbType.SmallInt, dto.IdTipoIdentificadorUsuario));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.NVarChar, dto.Valor, 250));
        command.Parameters.Add(CreateParameter("@valor_normalizado", SqlDbType.NVarChar, dto.ValorNormalizado, 250));
        command.Parameters.Add(CreateParameter("@es_principal", SqlDbType.Bit, dto.EsPrincipal));
        command.Parameters.Add(CreateParameter("@verificado", SqlDbType.Bit, dto.Verificado));
        command.Parameters.Add(CreateParameter("@fecha_verificacion_utc", SqlDbType.DateTime2, dto.FechaVerificacionUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUsuarioIdentificador, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_usuario_identificador", SqlDbType.BigInt, idUsuarioIdentificador));
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
