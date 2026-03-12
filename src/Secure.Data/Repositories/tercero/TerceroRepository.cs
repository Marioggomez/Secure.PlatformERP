using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Common;
using Secure.Platform.Contracts.Dtos.Tercero;
using Secure.Platform.Data.Repositories.Interfaces.Tercero;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Tercero;

/// <summary>
/// Repositorio ADO.NET para tercero.tercero con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class TerceroRepository : ITerceroRepository
{
    private const string SpListar = "tercero.usp_tercero_listar";
    private const string SpListarPaginado = "tercero.usp_tercero_listar_paginado";
    private const string SpObtener = "tercero.usp_tercero_obtener";
    private const string SpCrear = "tercero.usp_tercero_crear";
    private const string SpActualizar = "tercero.usp_tercero_actualizar";
    private const string SpDesactivar = "tercero.usp_tercero_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public TerceroRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<TerceroDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<TerceroDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(MapTercero(reader));
        }

        return result;
    }

    public async Task<PaginacionResultadoDto<TerceroListadoDto>> ListarPaginadoAsync(PaginacionRequestDto request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var size = request.Size < 5 ? 25 : Math.Min(request.Size, 500);

        var total = 0;
        var items = new List<TerceroListadoDto>();

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

        var totalParameter = new SqlParameter("@total_registros", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(totalParameter);

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new TerceroListadoDto
            {
                IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
                Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
                IdTipoPersona = reader.GetInt32(reader.GetOrdinal("id_tipo_persona")),
                TipoPersona = reader.IsDBNull(reader.GetOrdinal("tipo_persona")) ? string.Empty : reader.GetString(reader.GetOrdinal("tipo_persona")),
                NombrePrincipal = reader.IsDBNull(reader.GetOrdinal("nombre_principal")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombre_principal")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc"))
            });
        }

        if (totalParameter.Value != DBNull.Value)
        {
            total = Convert.ToInt32(totalParameter.Value);
        }

        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)size));
        return new PaginacionResultadoDto<TerceroListadoDto>
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

    public async Task<TerceroDto?> ObtenerAsync(long idTercero, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, idTercero));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return MapTercero(reader);
        }

        return null;
    }

    public async Task<long> CrearAsync(TerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@id_tipo_persona", SqlDbType.Int, dto.IdTipoPersona));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@segundo_nombre", SqlDbType.NVarChar, dto.SegundoNombre, 200));
        command.Parameters.Add(CreateParameter("@apellido", SqlDbType.NVarChar, dto.Apellido, 200));
        command.Parameters.Add(CreateParameter("@segundo_apellido", SqlDbType.NVarChar, dto.SegundoApellido, 200));
        command.Parameters.Add(CreateParameter("@razon_social", SqlDbType.NVarChar, dto.RazonSocial, 400));
        command.Parameters.Add(CreateParameter("@nombre_comercial", SqlDbType.NVarChar, dto.NombreComercial, 400));
        command.Parameters.Add(CreateParameter("@fecha_nacimiento", SqlDbType.Date, dto.FechaNacimiento));
        command.Parameters.Add(CreateParameter("@fecha_constitucion", SqlDbType.Date, dto.FechaConstitucion));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_por", SqlDbType.Int, dto.CreadoPor));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(TerceroDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, dto.IdTercero));
        command.Parameters.Add(CreateParameter("@codigo", SqlDbType.NVarChar, dto.Codigo, 50));
        command.Parameters.Add(CreateParameter("@id_tipo_persona", SqlDbType.Int, dto.IdTipoPersona));
        command.Parameters.Add(CreateParameter("@nombre", SqlDbType.NVarChar, dto.Nombre, 200));
        command.Parameters.Add(CreateParameter("@segundo_nombre", SqlDbType.NVarChar, dto.SegundoNombre, 200));
        command.Parameters.Add(CreateParameter("@apellido", SqlDbType.NVarChar, dto.Apellido, 200));
        command.Parameters.Add(CreateParameter("@segundo_apellido", SqlDbType.NVarChar, dto.SegundoApellido, 200));
        command.Parameters.Add(CreateParameter("@razon_social", SqlDbType.NVarChar, dto.RazonSocial, 400));
        command.Parameters.Add(CreateParameter("@nombre_comercial", SqlDbType.NVarChar, dto.NombreComercial, 400));
        command.Parameters.Add(CreateParameter("@fecha_nacimiento", SqlDbType.Date, dto.FechaNacimiento));
        command.Parameters.Add(CreateParameter("@fecha_constitucion", SqlDbType.Date, dto.FechaConstitucion));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        command.Parameters.Add(CreateParameter("@creado_por", SqlDbType.Int, dto.CreadoPor));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, dto.CreadoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idTercero, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_tercero", SqlDbType.BigInt, idTercero));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    private static TerceroDto MapTercero(SqlDataReader reader)
    {
        return new TerceroDto
        {
            IdTercero = reader.GetInt64(reader.GetOrdinal("id_tercero")),
            Codigo = reader.IsDBNull(reader.GetOrdinal("codigo")) ? string.Empty : reader.GetString(reader.GetOrdinal("codigo")),
            IdTipoPersona = reader.GetInt32(reader.GetOrdinal("id_tipo_persona")),
            Nombre = reader.IsDBNull(reader.GetOrdinal("nombre")) ? null : reader.GetString(reader.GetOrdinal("nombre")),
            SegundoNombre = reader.IsDBNull(reader.GetOrdinal("segundo_nombre")) ? null : reader.GetString(reader.GetOrdinal("segundo_nombre")),
            Apellido = reader.IsDBNull(reader.GetOrdinal("apellido")) ? null : reader.GetString(reader.GetOrdinal("apellido")),
            SegundoApellido = reader.IsDBNull(reader.GetOrdinal("segundo_apellido")) ? null : reader.GetString(reader.GetOrdinal("segundo_apellido")),
            RazonSocial = reader.IsDBNull(reader.GetOrdinal("razon_social")) ? null : reader.GetString(reader.GetOrdinal("razon_social")),
            NombreComercial = reader.IsDBNull(reader.GetOrdinal("nombre_comercial")) ? null : reader.GetString(reader.GetOrdinal("nombre_comercial")),
            FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("fecha_nacimiento")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_nacimiento")),
            FechaConstitucion = reader.IsDBNull(reader.GetOrdinal("fecha_constitucion")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha_constitucion")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
            CreadoPor = reader.IsDBNull(reader.GetOrdinal("creado_por")) ? null : reader.GetInt32(reader.GetOrdinal("creado_por")),
            CreadoUtc = reader.GetDateTime(reader.GetOrdinal("creado_utc")),
            VersionFila = reader.IsDBNull(reader.GetOrdinal("version_fila")) ? Array.Empty<byte>() : (byte[])reader.GetValue(reader.GetOrdinal("version_fila"))
        };
    }

    private static string NormalizeSortBy(string? sortBy)
    {
        var normalized = (sortBy ?? string.Empty).Trim().ToLowerInvariant();
        return normalized switch
        {
            "id_tercero" => "id_tercero",
            "codigo" => "codigo",
            "id_tipo_persona" => "id_tipo_persona",
            "tipo_persona" => "tipo_persona",
            "nombre_principal" => "nombre_principal",
            "activo" => "activo",
            "creado_utc" => "creado_utc",
            _ => "id_tercero"
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
