using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Organizacion;
using Secure.Platform.Data.Repositories.Interfaces.Organizacion;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Organizacion;

/// <summary>
/// Repositorio ADO.NET para organizacion.empresa con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class EmpresaRepository : IEmpresaRepository
{
    private const string SpListar = "organizacion.usp_empresa_listar";
    private const string SpListarPaginado = "organizacion.usp_empresa_listar_paginado";
    private const string SpObtener = "organizacion.usp_empresa_obtener";
    private const string SpCrear = "organizacion.usp_empresa_crear";
    private const string SpActualizar = "organizacion.usp_empresa_actualizar";
    private const string SpDesactivar = "organizacion.usp_empresa_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public EmpresaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<EmpresaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<EmpresaDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new EmpresaDto
            {
                IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
                IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                NombreLegal = reader.IsDBNull(reader.GetOrdinal("nombre_legal")) ? null : reader.GetString(reader.GetOrdinal("nombre_legal")),
                IdTipoEmpresa = reader.GetInt16(reader.GetOrdinal("id_tipo_empresa")),
                IdEstadoEmpresa = reader.GetInt16(reader.GetOrdinal("id_estado_empresa")),
                IdentificacionFiscal = reader.IsDBNull(reader.GetOrdinal("identificacion_fiscal")) ? null : reader.GetString(reader.GetOrdinal("identificacion_fiscal")),
                MonedaBase = reader.IsDBNull(reader.GetOrdinal("moneda_base")) ? null : reader.GetString(reader.GetOrdinal("moneda_base")),
                ZonaHoraria = reader.IsDBNull(reader.GetOrdinal("zona_horaria")) ? null : reader.GetString(reader.GetOrdinal("zona_horaria")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
                ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
                VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            });
        }

        return result;
    }

    public async Task<PaginacionResultadoDto<EmpresaListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var size = request.Size < 5 ? 25 : Math.Min(request.Size, 500);
        var scopedTenantId = SqlScopeContext.Current?.IdTenant ?? request.IdTenant;

        var total = 0;
        var items = new List<EmpresaListadoDto>();

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
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, scopedTenantId));

        var totalParameter = new SqlParameter("@total_registros", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParameter);

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new EmpresaListadoDto
            {
                IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
                IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                NombreLegal = reader.IsDBNull(reader.GetOrdinal("nombre_legal")) ? null : reader.GetString(reader.GetOrdinal("nombre_legal")),
                IdentificacionFiscal = reader.IsDBNull(reader.GetOrdinal("identificacion_fiscal")) ? null : reader.GetString(reader.GetOrdinal("identificacion_fiscal")),
                Estado = reader.IsDBNull(reader.GetOrdinal("estado")) ? string.Empty : reader.GetString(reader.GetOrdinal("estado")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }

        if (totalParameter.Value != DBNull.Value)
        {
            total = Convert.ToInt32(totalParameter.Value);
        }

        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)size));
        return new PaginacionResultadoDto<EmpresaListadoDto>
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

    public async Task<EmpresaDto?> ObtenerAsync(long idEmpresa, CancellationToken cancellationToken)
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
            return new EmpresaDto
            {
                IdEmpresa = reader.GetInt64(reader.GetOrdinal("id_empresa")),
                IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre")),
                NombreLegal = reader.IsDBNull(reader.GetOrdinal("nombre_legal")) ? null : reader.GetString(reader.GetOrdinal("nombre_legal")),
                IdTipoEmpresa = reader.GetInt16(reader.GetOrdinal("id_tipo_empresa")),
                IdEstadoEmpresa = reader.GetInt16(reader.GetOrdinal("id_estado_empresa")),
                IdentificacionFiscal = reader.IsDBNull(reader.GetOrdinal("identificacion_fiscal")) ? null : reader.GetString(reader.GetOrdinal("identificacion_fiscal")),
                MonedaBase = reader.IsDBNull(reader.GetOrdinal("moneda_base")) ? null : reader.GetString(reader.GetOrdinal("moneda_base")),
                ZonaHoraria = reader.IsDBNull(reader.GetOrdinal("zona_horaria")) ? null : reader.GetString(reader.GetOrdinal("zona_horaria")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
                ActualizadoUtc = reader.IsDBNull(reader.GetOrdinal("actualizado_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("actualizado_utc")),
                VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
            };
        }

        return null;
    }

    public async Task<long> CrearAsync(EmpresaDto dto, CancellationToken cancellationToken)
    {
        var scopedTenantId = SqlScopeContext.Current?.IdTenant ?? dto.IdTenant;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, scopedTenantId));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 250));
        command.Parameters.Add(CreateParameter("@nombre_legal", SqlDbType.NVarChar, dto.NombreLegal, 300));
        command.Parameters.Add(CreateParameter("@id_tipo_empresa", SqlDbType.SmallInt, dto.IdTipoEmpresa));
        command.Parameters.Add(CreateParameter("@id_estado_empresa", SqlDbType.SmallInt, dto.IdEstadoEmpresa));
        command.Parameters.Add(CreateParameter("@identificacion_fiscal", SqlDbType.NVarChar, dto.IdentificacionFiscal, 50));
        command.Parameters.Add(CreateParameter("@moneda_base", SqlDbType.Char, dto.MonedaBase, 3));
        command.Parameters.Add(CreateParameter("@zona_horaria", SqlDbType.NVarChar, dto.ZonaHoraria, 80));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        command.Parameters.Add(CreateParameter("@actualizado_utc", SqlDbType.DateTime2, dto.ActualizadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(EmpresaDto dto, CancellationToken cancellationToken)
    {
        var scopedTenantId = SqlScopeContext.Current?.IdTenant ?? dto.IdTenant;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, dto.IdEmpresa));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, scopedTenantId));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 250));
        command.Parameters.Add(CreateParameter("@nombre_legal", SqlDbType.NVarChar, dto.NombreLegal, 300));
        command.Parameters.Add(CreateParameter("@id_tipo_empresa", SqlDbType.SmallInt, dto.IdTipoEmpresa));
        command.Parameters.Add(CreateParameter("@id_estado_empresa", SqlDbType.SmallInt, dto.IdEstadoEmpresa));
        command.Parameters.Add(CreateParameter("@identificacion_fiscal", SqlDbType.NVarChar, dto.IdentificacionFiscal, 50));
        command.Parameters.Add(CreateParameter("@moneda_base", SqlDbType.Char, dto.MonedaBase, 3));
        command.Parameters.Add(CreateParameter("@zona_horaria", SqlDbType.NVarChar, dto.ZonaHoraria, 80));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
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

    private static string NormalizeSortBy(string? sortBy)
    {
        var normalized = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "id_empresa" => "id_empresa",
            "codigo" => "codigo",
            "nombre" => "nombre",
            "nombre_legal" => "nombre_legal",
            "identificacion_fiscal" => "identificacion_fiscal",
            "estado" => "estado",
            _ => "id_empresa"
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
