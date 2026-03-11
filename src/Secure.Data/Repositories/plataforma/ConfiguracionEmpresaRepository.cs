using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Plataforma;
using Secure.Platform.Data.Repositories.Interfaces.Plataforma;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Plataforma;

/// <summary>
/// Repositorio ADO.NET para plataforma.configuracion_empresa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ConfiguracionEmpresaRepository : IConfiguracionEmpresaRepository
{
    private const string SpListar = "plataforma.usp_configuracion_empresa_listar";
    private const string SpObtener = "plataforma.usp_configuracion_empresa_obtener";
    private const string SpCrear = "plataforma.usp_configuracion_empresa_crear";
    private const string SpActualizar = "plataforma.usp_configuracion_empresa_actualizar";
    private const string SpDesactivar = "plataforma.usp_configuracion_empresa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ConfiguracionEmpresaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ConfiguracionEmpresaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ConfiguracionEmpresaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ConfiguracionEmpresaDto
            {
            IdConfiguracionEmpresa = reader.GetInt64(reader.GetOrdinal("id_configuracion_empresa")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdParametroConfiguracion = reader.GetInt32(reader.GetOrdinal("id_parametro_configuracion")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? null : reader.GetString(reader.GetOrdinal("valor")),
            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_creacion"))
            });
        }
        return result;
    }

    public async Task<ConfiguracionEmpresaDto?> ObtenerAsync(long idConfiguracionEmpresa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_configuracion_empresa", SqlDbType.BigInt, idConfiguracionEmpresa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ConfiguracionEmpresaDto
            {
            IdConfiguracionEmpresa = reader.GetInt64(reader.GetOrdinal("id_configuracion_empresa")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdParametroConfiguracion = reader.GetInt32(reader.GetOrdinal("id_parametro_configuracion")),
            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? null : reader.GetString(reader.GetOrdinal("valor")),
            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("fecha_creacion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_creacion"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ConfiguracionEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_parametro_configuracion", SqlDbType.Int, dto.IdParametroConfiguracion));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.VarChar, dto.Valor, 500));
        command.Parameters.Add(CreateParameter("@fecha_creacion", SqlDbType.DateTime2, dto.FechaCreacion));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ConfiguracionEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_configuracion_empresa", SqlDbType.BigInt, dto.IdConfiguracionEmpresa));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_parametro_configuracion", SqlDbType.Int, dto.IdParametroConfiguracion));
        command.Parameters.Add(CreateParameter("@valor", SqlDbType.VarChar, dto.Valor, 500));
        command.Parameters.Add(CreateParameter("@fecha_creacion", SqlDbType.DateTime2, dto.FechaCreacion));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idConfiguracionEmpresa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_configuracion_empresa", SqlDbType.BigInt, idConfiguracionEmpresa));
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
