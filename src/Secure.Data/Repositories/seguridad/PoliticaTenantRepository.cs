using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.politica_tenant con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaTenantRepository : IPoliticaTenantRepository
{
    private const string SpListar = "seguridad.usp_politica_tenant_listar";
    private const string SpObtener = "seguridad.usp_politica_tenant_obtener";
    private const string SpCrear = "seguridad.usp_politica_tenant_crear";
    private const string SpActualizar = "seguridad.usp_politica_tenant_actualizar";
    private const string SpDesactivar = "seguridad.usp_politica_tenant_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PoliticaTenantRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PoliticaTenantDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PoliticaTenantDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PoliticaTenantDto
            {
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            TimeoutInactividadMin = reader.GetInt32(reader.GetOrdinal("timeout_inactividad_min")),
            TimeoutAbsolutoMin = reader.GetInt32(reader.GetOrdinal("timeout_absoluto_min")),
            LongitudMinimaClave = reader.GetByte(reader.GetOrdinal("longitud_minima_clave")),
            RequiereMayuscula = reader.GetBoolean(reader.GetOrdinal("requiere_mayuscula")),
            RequiereMinuscula = reader.GetBoolean(reader.GetOrdinal("requiere_minuscula")),
            RequiereNumero = reader.GetBoolean(reader.GetOrdinal("requiere_numero")),
            RequiereEspecial = reader.GetBoolean(reader.GetOrdinal("requiere_especial")),
            HistorialClaves = reader.GetByte(reader.GetOrdinal("historial_claves")),
            MaxIntentosLogin = reader.GetByte(reader.GetOrdinal("max_intentos_login")),
            MinutosBloqueo = reader.GetInt32(reader.GetOrdinal("minutos_bloqueo")),
            MfaObligatorio = reader.GetBoolean(reader.GetOrdinal("mfa_obligatorio")),
            PermiteLoginLocal = reader.GetBoolean(reader.GetOrdinal("permite_login_local")),
            PermiteSso = reader.GetBoolean(reader.GetOrdinal("permite_sso")),
            RequiereMfaAprobaciones = reader.GetBoolean(reader.GetOrdinal("requiere_mfa_aprobaciones")),
            RequierePoliticaIp = reader.GetBoolean(reader.GetOrdinal("requiere_politica_ip")),
            LimiteRatePorMinuto = reader.GetInt32(reader.GetOrdinal("limite_rate_por_minuto")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<PoliticaTenantDto?> ObtenerAsync(long idTenant, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PoliticaTenantDto
            {
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            TimeoutInactividadMin = reader.GetInt32(reader.GetOrdinal("timeout_inactividad_min")),
            TimeoutAbsolutoMin = reader.GetInt32(reader.GetOrdinal("timeout_absoluto_min")),
            LongitudMinimaClave = reader.GetByte(reader.GetOrdinal("longitud_minima_clave")),
            RequiereMayuscula = reader.GetBoolean(reader.GetOrdinal("requiere_mayuscula")),
            RequiereMinuscula = reader.GetBoolean(reader.GetOrdinal("requiere_minuscula")),
            RequiereNumero = reader.GetBoolean(reader.GetOrdinal("requiere_numero")),
            RequiereEspecial = reader.GetBoolean(reader.GetOrdinal("requiere_especial")),
            HistorialClaves = reader.GetByte(reader.GetOrdinal("historial_claves")),
            MaxIntentosLogin = reader.GetByte(reader.GetOrdinal("max_intentos_login")),
            MinutosBloqueo = reader.GetInt32(reader.GetOrdinal("minutos_bloqueo")),
            MfaObligatorio = reader.GetBoolean(reader.GetOrdinal("mfa_obligatorio")),
            PermiteLoginLocal = reader.GetBoolean(reader.GetOrdinal("permite_login_local")),
            PermiteSso = reader.GetBoolean(reader.GetOrdinal("permite_sso")),
            RequiereMfaAprobaciones = reader.GetBoolean(reader.GetOrdinal("requiere_mfa_aprobaciones")),
            RequierePoliticaIp = reader.GetBoolean(reader.GetOrdinal("requiere_politica_ip")),
            LimiteRatePorMinuto = reader.GetInt32(reader.GetOrdinal("limite_rate_por_minuto")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PoliticaTenantDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@timeout_inactividad_min", SqlDbType.Int, dto.TimeoutInactividadMin));
        command.Parameters.Add(CreateParameter("@timeout_absoluto_min", SqlDbType.Int, dto.TimeoutAbsolutoMin));
        command.Parameters.Add(CreateParameter("@longitud_minima_clave", SqlDbType.TinyInt, dto.LongitudMinimaClave));
        command.Parameters.Add(CreateParameter("@requiere_mayuscula", SqlDbType.Bit, dto.RequiereMayuscula));
        command.Parameters.Add(CreateParameter("@requiere_minuscula", SqlDbType.Bit, dto.RequiereMinuscula));
        command.Parameters.Add(CreateParameter("@requiere_numero", SqlDbType.Bit, dto.RequiereNumero));
        command.Parameters.Add(CreateParameter("@requiere_especial", SqlDbType.Bit, dto.RequiereEspecial));
        command.Parameters.Add(CreateParameter("@historial_claves", SqlDbType.TinyInt, dto.HistorialClaves));
        command.Parameters.Add(CreateParameter("@max_intentos_login", SqlDbType.TinyInt, dto.MaxIntentosLogin));
        command.Parameters.Add(CreateParameter("@minutos_bloqueo", SqlDbType.Int, dto.MinutosBloqueo));
        command.Parameters.Add(CreateParameter("@mfa_obligatorio", SqlDbType.Bit, dto.MfaObligatorio));
        command.Parameters.Add(CreateParameter("@permite_login_local", SqlDbType.Bit, dto.PermiteLoginLocal));
        command.Parameters.Add(CreateParameter("@permite_sso", SqlDbType.Bit, dto.PermiteSso));
        command.Parameters.Add(CreateParameter("@requiere_mfa_aprobaciones", SqlDbType.Bit, dto.RequiereMfaAprobaciones));
        command.Parameters.Add(CreateParameter("@requiere_politica_ip", SqlDbType.Bit, dto.RequierePoliticaIp));
        command.Parameters.Add(CreateParameter("@limite_rate_por_minuto", SqlDbType.Int, dto.LimiteRatePorMinuto));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PoliticaTenantDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@timeout_inactividad_min", SqlDbType.Int, dto.TimeoutInactividadMin));
        command.Parameters.Add(CreateParameter("@timeout_absoluto_min", SqlDbType.Int, dto.TimeoutAbsolutoMin));
        command.Parameters.Add(CreateParameter("@longitud_minima_clave", SqlDbType.TinyInt, dto.LongitudMinimaClave));
        command.Parameters.Add(CreateParameter("@requiere_mayuscula", SqlDbType.Bit, dto.RequiereMayuscula));
        command.Parameters.Add(CreateParameter("@requiere_minuscula", SqlDbType.Bit, dto.RequiereMinuscula));
        command.Parameters.Add(CreateParameter("@requiere_numero", SqlDbType.Bit, dto.RequiereNumero));
        command.Parameters.Add(CreateParameter("@requiere_especial", SqlDbType.Bit, dto.RequiereEspecial));
        command.Parameters.Add(CreateParameter("@historial_claves", SqlDbType.TinyInt, dto.HistorialClaves));
        command.Parameters.Add(CreateParameter("@max_intentos_login", SqlDbType.TinyInt, dto.MaxIntentosLogin));
        command.Parameters.Add(CreateParameter("@minutos_bloqueo", SqlDbType.Int, dto.MinutosBloqueo));
        command.Parameters.Add(CreateParameter("@mfa_obligatorio", SqlDbType.Bit, dto.MfaObligatorio));
        command.Parameters.Add(CreateParameter("@permite_login_local", SqlDbType.Bit, dto.PermiteLoginLocal));
        command.Parameters.Add(CreateParameter("@permite_sso", SqlDbType.Bit, dto.PermiteSso));
        command.Parameters.Add(CreateParameter("@requiere_mfa_aprobaciones", SqlDbType.Bit, dto.RequiereMfaAprobaciones));
        command.Parameters.Add(CreateParameter("@requiere_politica_ip", SqlDbType.Bit, dto.RequierePoliticaIp));
        command.Parameters.Add(CreateParameter("@limite_rate_por_minuto", SqlDbType.Int, dto.LimiteRatePorMinuto));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idTenant, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
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
