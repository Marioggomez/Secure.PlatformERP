using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.paso_instancia_aprobacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PasoInstanciaAprobacionRepository : IPasoInstanciaAprobacionRepository
{
    private const string SpListar = "cumplimiento.usp_paso_instancia_aprobacion_listar";
    private const string SpObtener = "cumplimiento.usp_paso_instancia_aprobacion_obtener";
    private const string SpCrear = "cumplimiento.usp_paso_instancia_aprobacion_crear";
    private const string SpActualizar = "cumplimiento.usp_paso_instancia_aprobacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_paso_instancia_aprobacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PasoInstanciaAprobacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PasoInstanciaAprobacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PasoInstanciaAprobacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PasoInstanciaAprobacionDto
            {
            IdPasoInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_instancia_aprobacion")),
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            NivelOrden = reader.GetByte(reader.GetOrdinal("nivel_orden")),
            IdEstadoAprobacion = reader.GetInt16(reader.GetOrdinal("id_estado_aprobacion")),
            IniciadoUtc = reader.GetDateTime(reader.GetOrdinal("iniciado_utc")),
            ResueltoUtc = reader.IsDBNull(reader.GetOrdinal("resuelto_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("resuelto_utc"))
            });
        }
        return result;
    }

    public async Task<PasoInstanciaAprobacionDto?> ObtenerAsync(long idPasoInstanciaAprobacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_paso_instancia_aprobacion", SqlDbType.BigInt, idPasoInstanciaAprobacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PasoInstanciaAprobacionDto
            {
            IdPasoInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_instancia_aprobacion")),
            IdInstanciaAprobacion = reader.GetInt64(reader.GetOrdinal("id_instancia_aprobacion")),
            NivelOrden = reader.GetByte(reader.GetOrdinal("nivel_orden")),
            IdEstadoAprobacion = reader.GetInt16(reader.GetOrdinal("id_estado_aprobacion")),
            IniciadoUtc = reader.GetDateTime(reader.GetOrdinal("iniciado_utc")),
            ResueltoUtc = reader.IsDBNull(reader.GetOrdinal("resuelto_utc")) ? null : reader.GetDateTime(reader.GetOrdinal("resuelto_utc"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, dto.IdInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@nivel_orden", SqlDbType.TinyInt, dto.NivelOrden));
        command.Parameters.Add(CreateParameter("@id_estado_aprobacion", SqlDbType.SmallInt, dto.IdEstadoAprobacion));
        command.Parameters.Add(CreateParameter("@iniciado_utc", SqlDbType.DateTime2, dto.IniciadoUtc));
        command.Parameters.Add(CreateParameter("@resuelto_utc", SqlDbType.DateTime2, dto.ResueltoUtc));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PasoInstanciaAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_paso_instancia_aprobacion", SqlDbType.BigInt, dto.IdPasoInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@id_instancia_aprobacion", SqlDbType.BigInt, dto.IdInstanciaAprobacion));
        command.Parameters.Add(CreateParameter("@nivel_orden", SqlDbType.TinyInt, dto.NivelOrden));
        command.Parameters.Add(CreateParameter("@id_estado_aprobacion", SqlDbType.SmallInt, dto.IdEstadoAprobacion));
        command.Parameters.Add(CreateParameter("@iniciado_utc", SqlDbType.DateTime2, dto.IniciadoUtc));
        command.Parameters.Add(CreateParameter("@resuelto_utc", SqlDbType.DateTime2, dto.ResueltoUtc));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idPasoInstanciaAprobacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_paso_instancia_aprobacion", SqlDbType.BigInt, idPasoInstanciaAprobacion));
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
