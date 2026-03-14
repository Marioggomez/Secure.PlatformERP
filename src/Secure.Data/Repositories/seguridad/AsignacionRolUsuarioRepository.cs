using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.asignacion_rol_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AsignacionRolUsuarioRepository : IAsignacionRolUsuarioRepository
{
    private const string SpListar = "seguridad.usp_asignacion_rol_usuario_listar";
    private const string SpObtener = "seguridad.usp_asignacion_rol_usuario_obtener";
    private const string SpCrear = "seguridad.usp_asignacion_rol_usuario_crear";
    private const string SpActualizar = "seguridad.usp_asignacion_rol_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_asignacion_rol_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public AsignacionRolUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<AsignacionRolUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<AsignacionRolUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new AsignacionRolUsuarioDto
            {
            IdAsignacionRolUsuario = reader.GetInt64(reader.GetOrdinal("id_asignacion_rol_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdRol = reader.GetInt64(reader.GetOrdinal("id_rol")),
            IdAlcanceAsignacion = reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion")),
            IdGrupoEmpresarial = reader.IsDBNull(reader.GetOrdinal("id_grupo_empresarial")) ? null : reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            ConcedidoPor = reader.IsDBNull(reader.GetOrdinal("concedido_por")) ? null : reader.GetInt64(reader.GetOrdinal("concedido_por")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<AsignacionRolUsuarioDto?> ObtenerAsync(long idAsignacionRolUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_asignacion_rol_usuario", SqlDbType.BigInt, idAsignacionRolUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new AsignacionRolUsuarioDto
            {
            IdAsignacionRolUsuario = reader.GetInt64(reader.GetOrdinal("id_asignacion_rol_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdRol = reader.GetInt64(reader.GetOrdinal("id_rol")),
            IdAlcanceAsignacion = reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion")),
            IdGrupoEmpresarial = reader.IsDBNull(reader.GetOrdinal("id_grupo_empresarial")) ? null : reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            ConcedidoPor = reader.IsDBNull(reader.GetOrdinal("concedido_por")) ? null : reader.GetInt64(reader.GetOrdinal("concedido_por")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(AsignacionRolUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_rol", SqlDbType.BigInt, dto.IdRol));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion", SqlDbType.SmallInt, dto.IdAlcanceAsignacion));
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, dto.IdGrupoEmpresarial));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@concedido_por", SqlDbType.BigInt, dto.ConcedidoPor));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(AsignacionRolUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_asignacion_rol_usuario", SqlDbType.BigInt, dto.IdAsignacionRolUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_rol", SqlDbType.BigInt, dto.IdRol));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion", SqlDbType.SmallInt, dto.IdAlcanceAsignacion));
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, dto.IdGrupoEmpresarial));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@concedido_por", SqlDbType.BigInt, dto.ConcedidoPor));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return HasAffectedRows(affected);
    }

    public async Task<bool> DesactivarAsync(long idAsignacionRolUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_asignacion_rol_usuario", SqlDbType.BigInt, idAsignacionRolUsuario));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return HasAffectedRows(affected);
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }

    private static bool HasAffectedRows(object? result)
    {
        if (result is null || result == DBNull.Value)
        {
            return false;
        }

        if (result is int i)
        {
            return i > 0;
        }

        return Convert.ToInt32(result) > 0;
    }
}
