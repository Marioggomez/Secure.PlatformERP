using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.auditoria_operacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaOperacionRepository : IAuditoriaOperacionRepository
{
    private const string SpListar = "cumplimiento.usp_auditoria_operacion_listar";
    private const string SpObtener = "cumplimiento.usp_auditoria_operacion_obtener";
    private const string SpCrear = "cumplimiento.usp_auditoria_operacion_crear";
    private const string SpActualizar = "cumplimiento.usp_auditoria_operacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_auditoria_operacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AuditoriaOperacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AuditoriaOperacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AuditoriaOperacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AuditoriaOperacionDto
            {
            IdAuditoria = reader.GetInt64(reader.GetOrdinal("id_auditoria")),
            Tabla = reader.IsDBNull(reader.GetOrdinal("tabla")) ? null : reader.GetString(reader.GetOrdinal("tabla")),
            Operacion = reader.IsDBNull(reader.GetOrdinal("operacion")) ? null : reader.GetString(reader.GetOrdinal("operacion")),
            IdRegistro = reader.IsDBNull(reader.GetOrdinal("id_registro")) ? null : reader.GetInt64(reader.GetOrdinal("id_registro")),
            ValoresAnteriores = reader.IsDBNull(reader.GetOrdinal("valores_anteriores")) ? null : reader.GetString(reader.GetOrdinal("valores_anteriores")),
            ValoresNuevos = reader.IsDBNull(reader.GetOrdinal("valores_nuevos")) ? null : reader.GetString(reader.GetOrdinal("valores_nuevos")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            });
        }
        return result;
    }

    public async Task<AuditoriaOperacionDto?> ObtenerAsync(long idAuditoria, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_auditoria", SqlDbType.BigInt, idAuditoria));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AuditoriaOperacionDto
            {
            IdAuditoria = reader.GetInt64(reader.GetOrdinal("id_auditoria")),
            Tabla = reader.IsDBNull(reader.GetOrdinal("tabla")) ? null : reader.GetString(reader.GetOrdinal("tabla")),
            Operacion = reader.IsDBNull(reader.GetOrdinal("operacion")) ? null : reader.GetString(reader.GetOrdinal("operacion")),
            IdRegistro = reader.IsDBNull(reader.GetOrdinal("id_registro")) ? null : reader.GetInt64(reader.GetOrdinal("id_registro")),
            ValoresAnteriores = reader.IsDBNull(reader.GetOrdinal("valores_anteriores")) ? null : reader.GetString(reader.GetOrdinal("valores_anteriores")),
            ValoresNuevos = reader.IsDBNull(reader.GetOrdinal("valores_nuevos")) ? null : reader.GetString(reader.GetOrdinal("valores_nuevos")),
            Usuario = reader.IsDBNull(reader.GetOrdinal("usuario")) ? null : reader.GetString(reader.GetOrdinal("usuario")),
            CorrelationId = reader.IsDBNull(reader.GetOrdinal("correlation_id")) ? null : reader.GetGuid(reader.GetOrdinal("correlation_id")),
            Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AuditoriaOperacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@tabla", SqlDbType.VarChar, dto.Tabla, 100));
        command.Parameters.Add(CreateParameter("@operacion", SqlDbType.VarChar, dto.Operacion, 50));
        command.Parameters.Add(CreateParameter("@id_registro", SqlDbType.BigInt, dto.IdRegistro));
        command.Parameters.Add(CreateParameter("@valores_anteriores", SqlDbType.NVarChar, dto.ValoresAnteriores, -1));
        command.Parameters.Add(CreateParameter("@valores_nuevos", SqlDbType.NVarChar, dto.ValoresNuevos, -1));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 100));
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AuditoriaOperacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_auditoria", SqlDbType.BigInt, dto.IdAuditoria));
        command.Parameters.Add(CreateParameter("@tabla", SqlDbType.VarChar, dto.Tabla, 100));
        command.Parameters.Add(CreateParameter("@operacion", SqlDbType.VarChar, dto.Operacion, 50));
        command.Parameters.Add(CreateParameter("@id_registro", SqlDbType.BigInt, dto.IdRegistro));
        command.Parameters.Add(CreateParameter("@valores_anteriores", SqlDbType.NVarChar, dto.ValoresAnteriores, -1));
        command.Parameters.Add(CreateParameter("@valores_nuevos", SqlDbType.NVarChar, dto.ValoresNuevos, -1));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, dto.Usuario, 100));
        command.Parameters.Add(CreateParameter("@correlation_id", SqlDbType.UniqueIdentifier, dto.CorrelationId));
        command.Parameters.Add(CreateParameter("@fecha", SqlDbType.DateTime2, dto.Fecha));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idAuditoria, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_auditoria", SqlDbType.BigInt, idAuditoria));
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
