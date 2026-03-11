using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Organizacion;

/// <summary>
/// Repositorio ADO.NET para organizacion.grupo_empresarial_empresa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class GrupoEmpresarialEmpresaRepository : IGrupoEmpresarialEmpresaRepository
{
    private const string SpListar = "organizacion.usp_grupo_empresarial_empresa_listar";
    private const string SpObtener = "organizacion.usp_grupo_empresarial_empresa_obtener";
    private const string SpCrear = "organizacion.usp_grupo_empresarial_empresa_crear";
    private const string SpActualizar = "organizacion.usp_grupo_empresarial_empresa_actualizar";
    private const string SpDesactivar = "organizacion.usp_grupo_empresarial_empresa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public GrupoEmpresarialEmpresaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<GrupoEmpresarialEmpresaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<GrupoEmpresarialEmpresaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new GrupoEmpresarialEmpresaDto
            {
            IdGrupoEmpresarial = reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }
        return result;
    }

    public async Task<GrupoEmpresarialEmpresaDto?> ObtenerAsync(long idGrupoEmpresarial, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, idGrupoEmpresarial));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new GrupoEmpresarialEmpresaDto
            {
            IdGrupoEmpresarial = reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(GrupoEmpresarialEmpresaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, dto.IdGrupoEmpresarial));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idGrupoEmpresarial, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, idGrupoEmpresarial));
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
