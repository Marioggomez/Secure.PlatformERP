using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.politica_empresa_override con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PoliticaEmpresaOverrideRepository : IPoliticaEmpresaOverrideRepository
{
    private const string SpListar = "seguridad.usp_politica_empresa_override_listar";
    private const string SpObtener = "seguridad.usp_politica_empresa_override_obtener";
    private const string SpCrear = "seguridad.usp_politica_empresa_override_crear";
    private const string SpActualizar = "seguridad.usp_politica_empresa_override_actualizar";
    private const string SpDesactivar = "seguridad.usp_politica_empresa_override_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PoliticaEmpresaOverrideRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PoliticaEmpresaOverrideDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PoliticaEmpresaOverrideDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PoliticaEmpresaOverrideDto
            {
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            TimeoutInactividadMinOverride = reader.IsDBNull(reader.GetOrdinal("timeout_inactividad_min_override")) ? null : reader.GetInt32(reader.GetOrdinal("timeout_inactividad_min_override")),
            TimeoutAbsolutoMinOverride = reader.IsDBNull(reader.GetOrdinal("timeout_absoluto_min_override")) ? null : reader.GetInt32(reader.GetOrdinal("timeout_absoluto_min_override")),
            MfaObligatorioOverride = reader.IsDBNull(reader.GetOrdinal("mfa_obligatorio_override")) ? null : reader.GetBoolean(reader.GetOrdinal("mfa_obligatorio_override")),
            MaxIntentosLoginOverride = reader.IsDBNull(reader.GetOrdinal("max_intentos_login_override")) ? null : reader.GetByte(reader.GetOrdinal("max_intentos_login_override")),
            MinutosBloqueoOverride = reader.IsDBNull(reader.GetOrdinal("minutos_bloqueo_override")) ? null : reader.GetInt32(reader.GetOrdinal("minutos_bloqueo_override")),
            RequierePoliticaIpOverride = reader.IsDBNull(reader.GetOrdinal("requiere_politica_ip_override")) ? null : reader.GetBoolean(reader.GetOrdinal("requiere_politica_ip_override")),
            RequiereMfaAprobacionesOverride = reader.IsDBNull(reader.GetOrdinal("requiere_mfa_aprobaciones_override")) ? null : reader.GetBoolean(reader.GetOrdinal("requiere_mfa_aprobaciones_override")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }
        return result;
    }

    public async Task<PoliticaEmpresaOverrideDto?> ObtenerAsync(long idEmpresa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, idEmpresa));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PoliticaEmpresaOverrideDto
            {
            IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
            TimeoutInactividadMinOverride = reader.IsDBNull(reader.GetOrdinal("timeout_inactividad_min_override")) ? null : reader.GetInt32(reader.GetOrdinal("timeout_inactividad_min_override")),
            TimeoutAbsolutoMinOverride = reader.IsDBNull(reader.GetOrdinal("timeout_absoluto_min_override")) ? null : reader.GetInt32(reader.GetOrdinal("timeout_absoluto_min_override")),
            MfaObligatorioOverride = reader.IsDBNull(reader.GetOrdinal("mfa_obligatorio_override")) ? null : reader.GetBoolean(reader.GetOrdinal("mfa_obligatorio_override")),
            MaxIntentosLoginOverride = reader.IsDBNull(reader.GetOrdinal("max_intentos_login_override")) ? null : reader.GetByte(reader.GetOrdinal("max_intentos_login_override")),
            MinutosBloqueoOverride = reader.IsDBNull(reader.GetOrdinal("minutos_bloqueo_override")) ? null : reader.GetInt32(reader.GetOrdinal("minutos_bloqueo_override")),
            RequierePoliticaIpOverride = reader.IsDBNull(reader.GetOrdinal("requiere_politica_ip_override")) ? null : reader.GetBoolean(reader.GetOrdinal("requiere_politica_ip_override")),
            RequiereMfaAprobacionesOverride = reader.IsDBNull(reader.GetOrdinal("requiere_mfa_aprobaciones_override")) ? null : reader.GetBoolean(reader.GetOrdinal("requiere_mfa_aprobaciones_override")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@timeout_inactividad_min_override", SqlDbType.Int, dto.TimeoutInactividadMinOverride));
        command.Parameters.Add(CreateParameter("@timeout_absoluto_min_override", SqlDbType.Int, dto.TimeoutAbsolutoMinOverride));
        command.Parameters.Add(CreateParameter("@mfa_obligatorio_override", SqlDbType.Bit, dto.MfaObligatorioOverride));
        command.Parameters.Add(CreateParameter("@max_intentos_login_override", SqlDbType.TinyInt, dto.MaxIntentosLoginOverride));
        command.Parameters.Add(CreateParameter("@minutos_bloqueo_override", SqlDbType.Int, dto.MinutosBloqueoOverride));
        command.Parameters.Add(CreateParameter("@requiere_politica_ip_override", SqlDbType.Bit, dto.RequierePoliticaIpOverride));
        command.Parameters.Add(CreateParameter("@requiere_mfa_aprobaciones_override", SqlDbType.Bit, dto.RequiereMfaAprobacionesOverride));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PoliticaEmpresaOverrideDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@timeout_inactividad_min_override", SqlDbType.Int, dto.TimeoutInactividadMinOverride));
        command.Parameters.Add(CreateParameter("@timeout_absoluto_min_override", SqlDbType.Int, dto.TimeoutAbsolutoMinOverride));
        command.Parameters.Add(CreateParameter("@mfa_obligatorio_override", SqlDbType.Bit, dto.MfaObligatorioOverride));
        command.Parameters.Add(CreateParameter("@max_intentos_login_override", SqlDbType.TinyInt, dto.MaxIntentosLoginOverride));
        command.Parameters.Add(CreateParameter("@minutos_bloqueo_override", SqlDbType.Int, dto.MinutosBloqueoOverride));
        command.Parameters.Add(CreateParameter("@requiere_politica_ip_override", SqlDbType.Bit, dto.RequierePoliticaIpOverride));
        command.Parameters.Add(CreateParameter("@requiere_mfa_aprobaciones_override", SqlDbType.Bit, dto.RequiereMfaAprobacionesOverride));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idEmpresa, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, idEmpresa));
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
