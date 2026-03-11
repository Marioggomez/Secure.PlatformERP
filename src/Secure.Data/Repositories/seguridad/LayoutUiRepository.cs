using System.Data;
using System.Data.SqlClient;
using System.Text;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para layouts de UI persistidos por usuario.
/// Autor: Mario Gomez.
/// </summary>
public sealed class LayoutUiRepository : ILayoutUiRepository
{
    private const string SpObtener = "seguridad.usp_filtro_dato_usuario_obtener_layout_ui";
    private const string SpGuardar = "seguridad.usp_filtro_dato_usuario_guardar_layout_ui";

    private readonly IDbConnectionFactory _connectionFactory;

    public LayoutUiRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ObtenerLayoutUiResponseDto> ObtenerAsync(long idUsuario, long idTenant, long? idEmpresa, string codigoLayout, CancellationToken cancellationToken)
    {
        var codigoEntidad = NormalizeLayoutCode(codigoLayout);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, idEmpresa));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, codigoEntidad, 256));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ObtenerLayoutUiResponseDto
            {
                Encontrado = true,
                CodigoEntidad = reader.IsDBNull(reader.GetOrdinal("codigo_entidad")) ? codigoEntidad : reader.GetString(reader.GetOrdinal("codigo_entidad")),
                LayoutPayload = reader.IsDBNull(reader.GetOrdinal("layout_payload")) ? null : reader.GetString(reader.GetOrdinal("layout_payload")),
                ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }

        return new ObtenerLayoutUiResponseDto
        {
            Encontrado = false,
            CodigoEntidad = codigoEntidad,
            LayoutPayload = null,
            ActualizadoUtc = null
        };
    }

    public async Task<GuardarLayoutUiResponseDto> GuardarAsync(GuardarLayoutUiRequestDto request, CancellationToken cancellationToken)
    {
        var codigoEntidad = NormalizeLayoutCode(request.CodigoLayout);
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpGuardar;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, request.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, request.IdTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, request.IdEmpresa));
        command.Parameters.Add(CreateParameter("@codigo_entidad", SqlDbType.NVarChar, codigoEntidad, 256));
        command.Parameters.Add(CreateParameter("@layout_payload", SqlDbType.NVarChar, request.LayoutPayload, 300));

        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return new GuardarLayoutUiResponseDto
        {
            Guardado = affected > 0,
            CodigoEntidad = codigoEntidad,
            ActualizadoUtc = DateTime.UtcNow
        };
    }

    private static string NormalizeLayoutCode(string? code)
    {
        var raw = string.IsNullOrWhiteSpace(code) ? "DEFAULT" : code.Trim().ToUpperInvariant();
        var builder = new StringBuilder(raw.Length);
        var previousUnderscore = false;

        foreach (var ch in raw)
        {
            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
                previousUnderscore = false;
            }
            else if (!previousUnderscore)
            {
                builder.Append('_');
                previousUnderscore = true;
            }
        }

        var sanitized = builder.ToString().Trim('_');
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            sanitized = "DEFAULT";
        }

        var full = $"UI_LAYOUT_{sanitized}";
        return full.Length > 256 ? full[..256] : full;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
