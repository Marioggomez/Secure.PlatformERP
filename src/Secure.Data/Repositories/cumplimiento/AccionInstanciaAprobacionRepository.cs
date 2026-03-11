using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.accion_instancia_aprobacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AccionInstanciaAprobacionRepository : IAccionInstanciaAprobacionRepository
{
    private const string SpListar = "cumplimiento.usp_accion_instancia_aprobacion_listar";
    private const string SpObtener = "cumplimiento.usp_accion_instancia_aprobacion_obtener";
    private const string SpCrear = "cumplimiento.usp_accion_instancia_aprobacion_crear";
    private const string SpActualizar = "cumplimiento.usp_accion_instancia_aprobacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_accion_instancia_aprobacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AccionInstanciaAprobacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AccionInstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AccionInstanciaAprobacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AccionInstanciaAprobacionDto
            {
            IdAccionInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_accion_instancia_aprobacion")),
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            IdPasoInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_instancia_aprobacion")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdAccionAprobacion = reader.GetInt16(reader.GetOrdinal("id_accion_aprobacion")),
            Comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? null : reader.GetString(reader.GetOrdinal("comentario")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc"))
            });
        }
        return result;
    }

    public async Task<AccionInstanciaAprobacionDto?> ObtenerAsync(long idAccionInstanciaAprobacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_accion_instancia_aprobacion", SqlDbType.BigInt, idAccionInstanciaAprobacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AccionInstanciaAprobacionDto
            {
            IdAccionInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_accion_instancia_aprobacion")),
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            IdPasoInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_instancia_aprobacion")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            IdAccionAprobacion = reader.GetInt16(reader.GetOrdinal("id_accion_aprobacion")),
            Comentario = reader.IsDBNull(reader.GetOrdinal("comentario")) ? null : reader.GetString(reader.GetOrdinal("comentario")),
            MfaValidado = reader.GetBoolean(reader.GetOrdinal("mfa_validado")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, dto.IdInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_paso_instancia_aprobacion", SqlDbType.BigInt, dto.IdPasoInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_accion_aprobacion", SqlDbType.SmallInt, dto.IdAccionAprobacion));
        command.Parameters.Add(CreateParameter("@comentario", SqlDbType.NVarChar, dto.Comentario, 500));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AccionInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_accion_instancia_aprobacion", SqlDbType.BigInt, dto.IdAccionInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, dto.IdInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_paso_instancia_aprobacion", SqlDbType.BigInt, dto.IdPasoInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@id_accion_aprobacion", SqlDbType.SmallInt, dto.IdAccionAprobacion));
        command.Parameters.Add(CreateParameter("@comentario", SqlDbType.NVarChar, dto.Comentario, 500));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, dto.MfaValidado));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idAccionInstanciaAprobacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_accion_instancia_aprobacion", SqlDbType.BigInt, idAccionInstanciaAprobacion));
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
