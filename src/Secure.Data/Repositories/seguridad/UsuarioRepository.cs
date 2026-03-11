using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class UsuarioRepository : IUsuarioRepository
{
    private const string SpListar = "seguridad.usp_usuario_listar";
    private const string SpListarPaginado = "seguridad.usp_usuario_listar_paginado";
    private const string SpObtener = "seguridad.usp_usuario_obtener";
    private const string SpCrear = "seguridad.usp_usuario_crear";
    private const string SpActualizar = "seguridad.usp_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public UsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<UsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new UsuarioDto
            {
                IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
                LoginPrincipal = reader.IsDBNull(reader.GetOrdinal("login_principal")) ? string.Empty : reader.GetString(reader.GetOrdinal("login_principal")),
                LoginNormalizado = reader.IsDBNull(reader.GetOrdinal("login_normalizado")) ? string.Empty : reader.GetString(reader.GetOrdinal("login_normalizado")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.IsDBNull(reader.GetOrdinal("apellido")) ? null : reader.GetString(reader.GetOrdinal("apellido")),
                NombreMostrar = reader.IsDBNull(reader.GetOrdinal("nombre_mostrar")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_mostrar")),
                CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString(reader.GetOrdinal("correo_electronico")),
                CorreoNormalizado = reader.IsDBNull(reader.GetOrdinal("correo_normalizado")) ? null : reader.GetString(reader.GetOrdinal("correo_normalizado")),
                TelefonoMovil = reader.IsDBNull(reader.GetOrdinal("telefono_movil")) ? null : reader.GetString(reader.GetOrdinal("telefono_movil")),
                Idioma = reader.IsDBNull(reader.GetOrdinal("idioma")) ? null : reader.GetString(reader.GetOrdinal("idioma")),
                ZonaHoraria = reader.IsDBNull(reader.GetOrdinal("zona_horaria")) ? null : reader.GetString(reader.GetOrdinal("zona_horaria")),
                IdEstadoUsuario = reader.GetInt16(reader.GetOrdinal("id_estado_usuario")),
                BloqueadoHastaUtc = reader.IsDBNull(reader.GetOrdinal("bloqueado_hasta_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("bloqueado_hasta_utc")),
                MfaHabilitado = reader.GetBoolean(reader.GetOrdinal("mfa_habilitado")),
                RequiereCambioClave = reader.GetBoolean(reader.GetOrdinal("requiere_cambio_clave")),
                UltimoAccesoUtc = reader.IsDBNull(reader.GetOrdinal("ultimo_acceso_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("ultimo_acceso_utc")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt64(reader.GetOrdinal("creado_por")),
                CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
                ActualizadoPor = reader.IsDBNull(reader.GetOrdinal("actualizado_por")) ? null : reader.GetInt64(reader.GetOrdinal("actualizado_por")),
                ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
                VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }

        return result;
    }

    public async Task<PaginacionResultadoDto<UsuarioListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var size = request.Size < 5 ? 25 : Math.Min(request.Size, 500);

        var total = 0;
        var items = new List<UsuarioListadoDto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListarPaginado;
        command.Parameters.Add(CreateParameter("@page", SqlDbType.Int, page));
        command.Parameters.Add(CreateParameter("@size", SqlDbType.Int, size));
        command.Parameters.Add(CreateParameter("@sort_by", SqlDbType.NVarChar, NormalizeSortBy(request.SortBy), 64));
        command.Parameters.Add(CreateParameter("@sort_dir", SqlDbType.VarChar, NormalizeSortDirection(request.SortDirection), 4));
        command.Parameters.Add(CreateParameter("@filter", SqlDbType.NVarChar, request.Filter, 200));
        command.Parameters.Add(CreateParameter("@filter_field", SqlDbType.NVarChar, request.FilterField, 64));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, request.IdTenant));

        var totalParameter = new SqlParameter("@total_registros", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParameter);

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new UsuarioListadoDto
            {
                IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
                IdTenant = reader.IsDBNull(reader.GetOrdinal("id_tenant")) ? null : reader.GetInt64(reader.GetOrdinal("id_tenant")),
                LoginPrincipal = reader.IsDBNull(reader.GetOrdinal("login_principal")) ? string.Empty : reader.GetString(reader.GetOrdinal("login_principal")),
                NombreMostrar = reader.IsDBNull(reader.GetOrdinal("nombre_mostrar")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_mostrar")),
                CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString(reader.GetOrdinal("correo_electronico")),
                Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado")),
                UltimoAccesoUtc = reader.IsDBNull(reader.GetOrdinal("ultimo_acceso_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("ultimo_acceso_utc")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }

        if (totalParameter.Value != DBNull.Value)
        {
            total = Convert.ToInt32(totalParameter.Value);
        }

        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)size));
        return new PaginacionResultadoDto<UsuarioListadoDto>
        {
            Page = page,
            Size = size,
            Total = total,
            TotalPages = totalPages,
            SortBy = NormalizeSortBy(request.SortBy),
            SortDirection = NormalizeSortDirection(request.SortDirection),
            Filter = request.Filter,
            FilterField = request.FilterField,
            Items = items
        };
    }

    public async Task<UsuarioDto?> ObtenerAsync(long idUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new UsuarioDto
            {
                IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? null : reader.GetString(reader.GetOrdinal("codigo")),
                LoginPrincipal = reader.IsDBNull(reader.GetOrdinal("login_principal")) ? string.Empty : reader.GetString(reader.GetOrdinal("login_principal")),
                LoginNormalizado = reader.IsDBNull(reader.GetOrdinal("login_normalizado")) ? string.Empty : reader.GetString(reader.GetOrdinal("login_normalizado")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.IsDBNull(reader.GetOrdinal("apellido")) ? null : reader.GetString(reader.GetOrdinal("apellido")),
                NombreMostrar = reader.IsDBNull(reader.GetOrdinal("nombre_mostrar")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_mostrar")),
                CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString(reader.GetOrdinal("correo_electronico")),
                CorreoNormalizado = reader.IsDBNull(reader.GetOrdinal("correo_normalizado")) ? null : reader.GetString(reader.GetOrdinal("correo_normalizado")),
                TelefonoMovil = reader.IsDBNull(reader.GetOrdinal("telefono_movil")) ? null : reader.GetString(reader.GetOrdinal("telefono_movil")),
                Idioma = reader.IsDBNull(reader.GetOrdinal("idioma")) ? null : reader.GetString(reader.GetOrdinal("idioma")),
                ZonaHoraria = reader.IsDBNull(reader.GetOrdinal("zona_horaria")) ? null : reader.GetString(reader.GetOrdinal("zona_horaria")),
                IdEstadoUsuario = reader.GetInt16(reader.GetOrdinal("id_estado_usuario")),
                BloqueadoHastaUtc = reader.IsDBNull(reader.GetOrdinal("bloqueado_hasta_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("bloqueado_hasta_utc")),
                MfaHabilitado = reader.GetBoolean(reader.GetOrdinal("mfa_habilitado")),
                RequiereCambioClave = reader.GetBoolean(reader.GetOrdinal("requiere_cambio_clave")),
                UltimoAccesoUtc = reader.IsDBNull(reader.GetOrdinal("ultimo_acceso_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("ultimo_acceso_utc")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt64(reader.GetOrdinal("creado_por")),
                CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
                ActualizadoPor = reader.IsDBNull(reader.GetOrdinal("actualizado_por")) ? null : reader.GetInt64(reader.GetOrdinal("actualizado_por")),
                ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
                VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }

        return null;
    }

    public async Task<long> CrearAsync(UsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 60));
        command.Parameters.Add(CreateParameter("@login_principal", SqlDbType.NVarChar, dto.LoginPrincipal, 120));
        command.Parameters.Add(CreateParameter("@login_normalizado", SqlDbType.NVarChar, dto.LoginNormalizado, 120));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 120));
        command.Parameters.Add(CreateParameter("@apellido", SqlDbType.NVarChar, dto.Apellido, 120));
        command.Parameters.Add(CreateParameter("@nombre_mostrar", SqlDbType.NVarChar, dto.NombreMostrar, 250));
        command.Parameters.Add(CreateParameter("@correo_electronico", SqlDbType.NVarChar, dto.CorreoElectronico, 250));
        command.Parameters.Add(CreateParameter("@correo_normalizado", SqlDbType.NVarChar, dto.CorreoNormalizado, 250));
        command.Parameters.Add(CreateParameter("@telefono_movil", SqlDbType.NVarChar, dto.TelefonoMovil, 50));
        command.Parameters.Add(CreateParameter("@idioma", SqlDbType.NVarChar, dto.Idioma, 10));
        command.Parameters.Add(CreateParameter("@zona_horaria", SqlDbType.NVarChar, dto.ZonaHoraria, 80));
        command.Parameters.Add(CreateParameter("@id_estado_usuario", SqlDbType.SmallInt, dto.IdEstadoUsuario));
        command.Parameters.Add(CreateParameter("@bloqueado_hasta_utc", SqlDbType.DateTime2, dto.BloqueadoHastaUtc));
        command.Parameters.Add(CreateParameter("@mfa_habilitado", SqlDbType.Bit, dto.MfaHabilitado));
        command.Parameters.Add(CreateParameter("@requiere_cambio_clave", SqlDbType.Bit, dto.RequiereCambioClave));
        command.Parameters.Add(CreateParameter("@ultimo_acceso_utc", SqlDbType.DateTime2, dto.UltimoAccesoUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_por", SqlDbType.BigInt, dto.CreadoPor));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_por", SqlDbType.BigInt, dto.ActualizadoPor));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(UsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 60));
        command.Parameters.Add(CreateParameter("@login_principal", SqlDbType.NVarChar, dto.LoginPrincipal, 120));
        command.Parameters.Add(CreateParameter("@login_normalizado", SqlDbType.NVarChar, dto.LoginNormalizado, 120));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 120));
        command.Parameters.Add(CreateParameter("@apellido", SqlDbType.NVarChar, dto.Apellido, 120));
        command.Parameters.Add(CreateParameter("@nombre_mostrar", SqlDbType.NVarChar, dto.NombreMostrar, 250));
        command.Parameters.Add(CreateParameter("@correo_electronico", SqlDbType.NVarChar, dto.CorreoElectronico, 250));
        command.Parameters.Add(CreateParameter("@correo_normalizado", SqlDbType.NVarChar, dto.CorreoNormalizado, 250));
        command.Parameters.Add(CreateParameter("@telefono_movil", SqlDbType.NVarChar, dto.TelefonoMovil, 50));
        command.Parameters.Add(CreateParameter("@idioma", SqlDbType.NVarChar, dto.Idioma, 10));
        command.Parameters.Add(CreateParameter("@zona_horaria", SqlDbType.NVarChar, dto.ZonaHoraria, 80));
        command.Parameters.Add(CreateParameter("@id_estado_usuario", SqlDbType.SmallInt, dto.IdEstadoUsuario));
        command.Parameters.Add(CreateParameter("@bloqueado_hasta_utc", SqlDbType.DateTime2, dto.BloqueadoHastaUtc));
        command.Parameters.Add(CreateParameter("@mfa_habilitado", SqlDbType.Bit, dto.MfaHabilitado));
        command.Parameters.Add(CreateParameter("@requiere_cambio_clave", SqlDbType.Bit, dto.RequiereCambioClave));
        command.Parameters.Add(CreateParameter("@ultimo_acceso_utc", SqlDbType.DateTime2, dto.UltimoAccesoUtc));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_por", SqlDbType.BigInt, dto.CreadoPor));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_por", SqlDbType.BigInt, dto.ActualizadoPor));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    private static string NormalizeSortBy(string? sortBy)
    {
        var normalized = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "id_usuario" => "id_usuario",
            "login_principal" => "login_principal",
            "nombre_mostrar" => "nombre_mostrar",
            "correo_electronico" => "correo_electronico",
            "estado" => "estado",
            "ultimo_acceso_utc" => "ultimo_acceso_utc",
            _ => "id_usuario"
        };
    }

    private static string NormalizeSortDirection(string? sortDirection)
    {
        return string.Equals(sortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
