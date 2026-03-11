using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.Cumplimiento;
using Secure.Platform.Data.Repositories.Interfaces.Cumplimiento;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Cumplimiento;

/// <summary>
/// Repositorio ADO.NET para cumplimiento.paso_perfil_aprobacion con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PasoPerfilAprobacionRepository : IPasoPerfilAprobacionRepository
{
    private const string SpListar = "cumplimiento.usp_paso_perfil_aprobacion_listar";
    private const string SpObtener = "cumplimiento.usp_paso_perfil_aprobacion_obtener";
    private const string SpCrear = "cumplimiento.usp_paso_perfil_aprobacion_crear";
    private const string SpActualizar = "cumplimiento.usp_paso_perfil_aprobacion_actualizar";
    private const string SpDesactivar = "cumplimiento.usp_paso_perfil_aprobacion_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public PasoPerfilAprobacionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<PasoPerfilAprobacionDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<PasoPerfilAprobacionDto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new PasoPerfilAprobacionDto
            {
            IdPasoPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_perfil_aprobacion")),
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            NivelOrden = reader.GetByte(reader.GetOrdinal("nivel_orden")),
            IdRolRequerido = reader.IsDBNull(reader.GetOrdinal("id_rol_requerido")) ? null : reader.GetInt64(reader.GetOrdinal("id_rol_requerido")),
            IdPrivilegioRequerido = reader.IsDBNull(reader.GetOrdinal("id_privilegio_requerido")) ? null : reader.GetInt64(reader.GetOrdinal("id_privilegio_requerido")),
            IdAlcanceAsignacionRequerido = reader.IsDBNull(reader.GetOrdinal("id_alcance_asignacion_requerido")) ? null : reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion_requerido")),
            AprobadoresMinimos = reader.GetByte(reader.GetOrdinal("aprobadores_minimos")),
            TodosDebenAprobar = reader.GetBoolean(reader.GetOrdinal("todos_deben_aprobar")),
            MontoMinimo = reader.IsDBNull(reader.GetOrdinal("monto_minimo")) ? null : reader.GetDecimal(reader.GetOrdinal("monto_minimo")),
            MontoMaximo = reader.IsDBNull(reader.GetOrdinal("monto_maximo")) ? null : reader.GetDecimal(reader.GetOrdinal("monto_maximo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            });
        }
        return result;
    }

    public async Task<PasoPerfilAprobacionDto?> ObtenerAsync(long idPasoPerfilAprobacion, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@id_paso_perfil_aprobacion", SqlDbType.BigInt, idPasoPerfilAprobacion));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new PasoPerfilAprobacionDto
            {
            IdPasoPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_paso_perfil_aprobacion")),
            IdPerfilAprobacion = reader.GetInt64(reader.GetOrdinal("id_perfil_aprobacion")),
            NivelOrden = reader.GetByte(reader.GetOrdinal("nivel_orden")),
            IdRolRequerido = reader.IsDBNull(reader.GetOrdinal("id_rol_requerido")) ? null : reader.GetInt64(reader.GetOrdinal("id_rol_requerido")),
            IdPrivilegioRequerido = reader.IsDBNull(reader.GetOrdinal("id_privilegio_requerido")) ? null : reader.GetInt64(reader.GetOrdinal("id_privilegio_requerido")),
            IdAlcanceAsignacionRequerido = reader.IsDBNull(reader.GetOrdinal("id_alcance_asignacion_requerido")) ? null : reader.GetInt16(reader.GetOrdinal("id_alcance_asignacion_requerido")),
            AprobadoresMinimos = reader.GetByte(reader.GetOrdinal("aprobadores_minimos")),
            TodosDebenAprobar = reader.GetBoolean(reader.GetOrdinal("todos_deben_aprobar")),
            MontoMinimo = reader.IsDBNull(reader.GetOrdinal("monto_minimo")) ? null : reader.GetDecimal(reader.GetOrdinal("monto_minimo")),
            MontoMaximo = reader.IsDBNull(reader.GetOrdinal("monto_maximo")) ? null : reader.GetDecimal(reader.GetOrdinal("monto_maximo")),
            Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        return null;
    }

    public async Task<long> CrearAsync(PasoPerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, dto.IdPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@nivel_orden", SqlDbType.TinyInt, dto.NivelOrden));
        command.Parameters.Add(CreateParameter("@id_rol_requerido", SqlDbType.BigInt, dto.IdRolRequerido));
        command.Parameters.Add(CreateParameter("@id_privilegio_requerido", SqlDbType.BigInt, dto.IdPrivilegioRequerido));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion_requerido", SqlDbType.SmallInt, dto.IdAlcanceAsignacionRequerido));
        command.Parameters.Add(CreateParameter("@aprobadores_minimos", SqlDbType.TinyInt, dto.AprobadoresMinimos));
        command.Parameters.Add(CreateParameter("@todos_deben_aprobar", SqlDbType.Bit, dto.TodosDebenAprobar));
        command.Parameters.Add(CreateParameter("@monto_minimo", SqlDbType.Decimal, dto.MontoMinimo));
        command.Parameters.Add(CreateParameter("@monto_maximo", SqlDbType.Decimal, dto.MontoMaximo));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return Convert.ToInt64(result);
    }

    public async Task<bool> ActualizarAsync(PasoPerfilAprobacionDto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
        command.Parameters.Add(CreateParameter("@id_paso_perfil_aprobacion", SqlDbType.BigInt, dto.IdPasoPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@id_perfil_aprobacion", SqlDbType.BigInt, dto.IdPerfilAprobacion));
        command.Parameters.Add(CreateParameter("@nivel_orden", SqlDbType.TinyInt, dto.NivelOrden));
        command.Parameters.Add(CreateParameter("@id_rol_requerido", SqlDbType.BigInt, dto.IdRolRequerido));
        command.Parameters.Add(CreateParameter("@id_privilegio_requerido", SqlDbType.BigInt, dto.IdPrivilegioRequerido));
        command.Parameters.Add(CreateParameter("@id_alcance_asignacion_requerido", SqlDbType.SmallInt, dto.IdAlcanceAsignacionRequerido));
        command.Parameters.Add(CreateParameter("@aprobadores_minimos", SqlDbType.TinyInt, dto.AprobadoresMinimos));
        command.Parameters.Add(CreateParameter("@todos_deben_aprobar", SqlDbType.Bit, dto.TodosDebenAprobar));
        command.Parameters.Add(CreateParameter("@monto_minimo", SqlDbType.Decimal, dto.MontoMinimo));
        command.Parameters.Add(CreateParameter("@monto_maximo", SqlDbType.Decimal, dto.MontoMaximo));
        command.Parameters.Add(CreateParameter("@activo", SqlDbType.Bit, dto.Activo));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync(long idPasoPerfilAprobacion, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@id_paso_perfil_aprobacion", SqlDbType.BigInt, idPasoPerfilAprobacion));
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
