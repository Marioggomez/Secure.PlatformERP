using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para seguridad.excepcion_permiso_usuario con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ExcepcionPermisoUsuarioRepository : IExcepcionPermisoUsuarioRepository
{
    private const string SpListar = "seguridad.usp_excepcion_permiso_usuario_listar";
    private const string SpObtener = "seguridad.usp_excepcion_permiso_usuario_obtener";
    private const string SpCrear = "seguridad.usp_excepcion_permiso_usuario_crear";
    private const string SpActualizar = "seguridad.usp_excepcion_permiso_usuario_actualizar";
    private const string SpDesactivar = "seguridad.usp_excepcion_permiso_usuario_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ExcepcionPermisoUsuarioRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ExcepcionPermisoUsuarioDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<ExcepcionPermisoUsuarioDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ExcepcionPermisoUsuarioDto
            {
            IdExcepcionPermisoUsuario = reader.GetInt64(reader.GetOrdinal("id_excepcion_permiso_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            IdEfectoPermiso = reader.GetInt16(reader.GetOrdinal("id_efecto_permiso")),
            IdAlcanceAsignacion = reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion")),
            IdGrupoEmpresarial = reader.IsDBNull(reader.GetOrdinal("id_grupo_empresarial")) ? null : reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            ConcedidoPor = reader.IsDBNull(reader.GetOrdinal("concedido_por")) ? null : reader.GetInt64(reader.GetOrdinal("concedido_por")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            });
        }
        return result;
    }

    public async Task<ExcepcionPermisoUsuarioDto?> ObtenerAsync(long idExcepcionPermisoUsuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_excepcion_permiso_usuario", SqlDbType.BigInt, idExcepcionPermisoUsuario));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ExcepcionPermisoUsuarioDto
            {
            IdExcepcionPermisoUsuario = reader.GetInt64(reader.GetOrdinal("id_excepcion_permiso_usuario")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdPermiso = reader.GetInt32(reader.GetOrdinal("id_permiso")),
            IdEfectoPermiso = reader.GetInt16(reader.GetOrdinal("id_efecto_permiso")),
            IdAlcanceAsignacion = reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion")),
            IdGrupoEmpresarial = reader.IsDBNull(reader.GetOrdinal("id_grupo_empresarial")) ? null : reader.GetInt64(reader.GetOrdinal("id_grupo_empresarial")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            IdUnidadOrganizativa = reader.IsDBNull(reader.GetOrdinal("id_unidad_organizativa")) ? null : reader.GetInt64(reader.GetOrdinal("id_unidad_organizativa")),
            FechaInicioUtc = reader.GetDateTime(reader.GetOrdinal("fecha_inicio_utc")),
            FechaFinUtc = reader.IsDBNull(reader.GetOrdinal("fecha_fin_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_fin_utc")),
            ConcedidoPor = reader.IsDBNull(reader.GetOrdinal("concedido_por")) ? null : reader.GetInt64(reader.GetOrdinal("concedido_por")),
            Motivo = reader.IsDBNull(reader.GetOrdinal("motivo")) ? null : reader.GetString(reader.GetOrdinal("motivo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@id_efecto_permiso", SqlDbType.SmallInt, dto.IdEfectoPermiso));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion", SqlDbType.SmallInt, dto.IdAlcanceAsignacion));
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, dto.IdGrupoEmpresarial));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@concedido_por", SqlDbType.BigInt, dto.ConcedidoPor));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(ExcepcionPermisoUsuarioDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_excepcion_permiso_usuario", SqlDbType.BigInt, dto.IdExcepcionPermisoUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, dto.IdUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, dto.IdTenant));
        command.Parameters.Add(CreateParameter("@id_permiso", SqlDbType.Int, dto.IdPermiso));
        command.Parameters.Add(CreateParameter("@id_efecto_permiso", SqlDbType.SmallInt, dto.IdEfectoPermiso));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion", SqlDbType.SmallInt, dto.IdAlcanceAsignacion));
        command.Parameters.Add(CreateParameter("@id_grupo_empresarial", SqlDbType.BigInt, dto.IdGrupoEmpresarial));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_unidad_organizativa", SqlDbType.BigInt, dto.IdUnidadOrganizativa));
        command.Parameters.Add(CreateParameter("@fecha_inicio_utc", SqlDbType.DateTime2, dto.FechaInicioUtc));
        command.Parameters.Add(CreateParameter("@fecha_fin_utc", SqlDbType.DateTime2, dto.FechaFinUtc));
        command.Parameters.Add(CreateParameter("@concedido_por", SqlDbType.BigInt, dto.ConcedidoPor));
        command.Parameters.Add(CreateParameter("@motivo", SqlDbType.NVarChar, dto.Motivo, 300));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idExcepcionPermisoUsuario, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_excepcion_permiso_usuario", SqlDbType.BigInt, idExcepcionPermisoUsuario));
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
