using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Observabilidad;
using Secure.Platform.Data.Repositories.Interfaces.Observabilidad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Observabilidad;

/// <summary>
/// Repositorio ADO.NET para observabilidad.auditoria_reinicio_mesa_ayuda con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditoriaReinicioMesaAyudaRepository : IAuditoriaReinicioMesaAyudaRepository
{
    private const string SpListar = "observabilidad.usp_auditoria_reinicio_mesa_ayuda_listar";
    private const string SpObtener = "observabilidad.usp_auditoria_reinicio_mesa_ayuda_obtener";
    private const string SpCrear = "observabilidad.usp_auditoria_reinicio_mesa_ayuda_crear";
    private const string SpActualizar = "observabilidad.usp_auditoria_reinicio_mesa_ayuda_actualizar";
    private const string SpDesactivar = "observabilidad.usp_auditoria_reinicio_mesa_ayuda_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AuditoriaReinicioMesaAyudaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AuditoriaReinicioMesaAyudaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AuditoriaReinicioMesaAyudaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AuditoriaReinicioMesaAyudaDto
            {
            IdAuditoriaReinicioMesaAyuda = reader.GetInt64(reader.GetOrdinal("id_auditoria_reinicio_mesa_ayuda")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUsuarioAfectado = reader.GetInt64(reader.GetOrdinal("id_usuario_afectado")),
            IdUsuarioAdministrador = reader.GetInt64(reader.GetOrdinal("id_usuario_administrador")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc"))
            });
        }
        return result;
    }

    public async Task<AuditoriaReinicioMesaAyudaDto?> ObtenerAsync(long idAuditoriaReinicioMesaAyuda, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_auditoria_reinicio_mesa_ayuda", SqlDbType.BigInt, idAuditoriaReinicioMesaAyuda));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AuditoriaReinicioMesaAyudaDto
            {
            IdAuditoriaReinicioMesaAyuda = reader.GetInt64(reader.GetOrdinal("id_auditoria_reinicio_mesa_ayuda")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUsuarioAfectado = reader.GetInt64(reader.GetOrdinal("id_usuario_afectado")),
            IdUsuarioAdministrador = reader.GetInt64(reader.GetOrdinal("id_usuario_administrador")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            IpOrigen = reader.IsDBNull(reader.GetOrdinal("ip_origen")) ? null : reader.GetString(reader.GetOrdinal("ip_origen")),
            AgenteUsuario = reader.IsDBNull(reader.GetOrdinal("agente_usuario")) ? null : reader.GetString(reader.GetOrdinal("agente_usuario")),
            FechaUtc = reader.GetDateTime(reader.GetOrdinal("fecha_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_usuario_afectado", SqlDbType.BigInt, dto.IdUsuarioAfectado));
        command.Parameters.Add(CreateParameter("@id_usuario_administrador", SqlDbType.BigInt, dto.IdUsuarioAdministrador));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_auditoria_reinicio_mesa_ayuda", SqlDbType.BigInt, dto.IdAuditoriaReinicioMesaAyuda));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_usuario_afectado", SqlDbType.BigInt, dto.IdUsuarioAfectado));
        command.Parameters.Add(CreateParameter("@id_usuario_administrador", SqlDbType.BigInt, dto.IdUsuarioAdministrador));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, dto.IpOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, dto.AgenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@fecha_utc", SqlDbType.DateTime2, dto.FechaUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idAuditoriaReinicioMesaAyuda, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_auditoria_reinicio_mesa_ayuda", SqlDbType.BigInt, idAuditoriaReinicioMesaAyuda));
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
