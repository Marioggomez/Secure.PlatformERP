using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Repositories.Models.Seguridad;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.Seguridad;

/// <summary>
/// Repositorio ADO.NET para flujo IAM de autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class IamAuthRepository : IIamAuthRepository
{
    private const string SpObtenerUsuario = "seguridad.usp_auth_obtener_usuario_para_autenticacion";
    private const string SpCrearFlujo = "seguridad.usp_auth_crear_flujo_autenticacion";
    private const string SpMarcarFlujoUsado = "seguridad.usp_auth_marcar_flujo_autenticacion_usado";
    private const string SpCrearDesafioMfa = "seguridad.usp_auth_crear_desafio_mfa";
    private const string SpObtenerDesafioMfa = "seguridad.usp_auth_obtener_desafio_mfa";
    private const string SpIncrementarIntentoMfa = "seguridad.usp_auth_incrementar_intento_desafio_mfa";
    private const string SpMarcarDesafioMfaValidado = "seguridad.usp_auth_marcar_desafio_mfa_validado";
    private const string SpCrearSesion = "seguridad.usp_auth_crear_sesion_usuario";
    private const string SpObtenerPermisos = "seguridad.usp_auth_obtener_permisos_usuario";
    private const string SpObtenerRecursosUi = "seguridad.usp_auth_obtener_recursos_ui_usuario";
    private const string SpCrearFlujoRestablecimiento = "seguridad.usp_auth_crear_flujo_restablecimiento_clave";
    private const string SpCrearTokenRestablecimiento = "seguridad.usp_auth_crear_token_restablecimiento_clave";
    private const string SpObtenerTokenRestablecimiento = "seguridad.usp_auth_obtener_token_restablecimiento_por_hash";
    private const string SpConsumirTokenRestablecimiento = "seguridad.usp_auth_consumir_token_restablecimiento";
    private const string SpActualizarClaveUsuario = "seguridad.usp_auth_actualizar_clave_usuario";

    private readonly IDbConnectionFactory _connectionFactory;

    public IamAuthRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<UsuarioAutenticacionData?> ObtenerUsuarioParaAutenticacionAsync(string tenantCodigo, string identificador, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtenerUsuario;
        command.Parameters.Add(CreateParameter("@tenant_codigo", SqlDbType.NVarChar, tenantCodigo, 50));
        command.Parameters.Add(CreateParameter("@identificador", SqlDbType.NVarChar, identificador, 250));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        return new UsuarioAutenticacionData
        {
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            TenantCodigo = reader.GetString(reader.GetOrdinal("tenant_codigo")),
            LoginPrincipal = reader.GetString(reader.GetOrdinal("login_principal")),
            NombreMostrar = reader.GetString(reader.GetOrdinal("nombre_mostrar")),
            CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString(reader.GetOrdinal("correo_electronico")),
            MfaHabilitado = reader.GetBoolean(reader.GetOrdinal("mfa_habilitado")),
            RequiereCambioClave = reader.GetBoolean(reader.GetOrdinal("requiere_cambio_clave")),
            IdEstadoUsuario = reader.GetInt16(reader.GetOrdinal("id_estado_usuario")),
            ActivoUsuario = reader.GetBoolean(reader.GetOrdinal("activo_usuario")),
            HashClave = (byte[])reader["hash_clave"],
            SaltClave = (byte[])reader["salt_clave"],
            AlgoritmoClave = reader.GetString(reader.GetOrdinal("algoritmo_clave")),
            IteracionesClave = reader.GetInt32(reader.GetOrdinal("iteraciones_clave")),
            ActivoCredencial = reader.GetBoolean(reader.GetOrdinal("activo_credencial"))
        };
    }

    public async Task<Guid> CrearFlujoAutenticacionAsync(
        Guid idFlujoAutenticacion,
        long idUsuario,
        long idTenant,
        bool mfaRequerido,
        bool mfaValidado,
        DateTime expiraEnUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? huellaDispositivo,
        string? solicitudId,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrearFlujo;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, idFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
        command.Parameters.Add(CreateParameter("@mfa_requerido", SqlDbType.Bit, mfaRequerido));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, mfaValidado));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, expiraEnUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, ipOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, agenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, huellaDispositivo, 200));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, solicitudId, 64));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseGuid(scalar, idFlujoAutenticacion);
    }

    public async Task<bool> MarcarFlujoAutenticacionUsadoAsync(Guid idFlujoAutenticacion, bool mfaValidado, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpMarcarFlujoUsado;
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, idFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, mfaValidado));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseInt(scalar) > 0;
    }

    public async Task<Guid> CrearDesafioMfaAsync(
        Guid idDesafioMfa,
        long idUsuario,
        long idTenant,
        long? idEmpresa,
        Guid idFlujoAutenticacion,
        short idPropositoDesafioMfa,
        short idCanalNotificacion,
        string? codigoAccion,
        byte[] otpHash,
        byte[] otpSalt,
        DateTime expiraEnUtc,
        short maxIntentos,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrearDesafioMfa;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, idEmpresa));
        command.Parameters.Add(CreateParameter("@id_flujo_autenticacion", SqlDbType.UniqueIdentifier, idFlujoAutenticacion));
        command.Parameters.Add(CreateParameter("@id_proposito_desafio_mfa", SqlDbType.SmallInt, idPropositoDesafioMfa));
        command.Parameters.Add(CreateParameter("@id_canal_notificacion", SqlDbType.SmallInt, idCanalNotificacion));
        command.Parameters.Add(CreateParameter("@codigo_accion", SqlDbType.NVarChar, codigoAccion, 100));
        command.Parameters.Add(CreateParameter("@otp_hash", SqlDbType.Binary, otpHash, 32));
        command.Parameters.Add(CreateParameter("@otp_salt", SqlDbType.VarBinary, otpSalt, 16));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, expiraEnUtc));
        command.Parameters.Add(CreateParameter("@max_intentos", SqlDbType.SmallInt, maxIntentos));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseGuid(scalar, idDesafioMfa);
    }

    public async Task<DesafioMfaData?> ObtenerDesafioMfaAsync(Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtenerDesafioMfa;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        return new DesafioMfaData
        {
            IdDesafioMfa = reader.GetGuid(reader.GetOrdinal("id_desafio_mfa")),
            IdFlujoAutenticacion = reader.IsDBNull(reader.GetOrdinal("id_flujo_autenticacion")) ? null : reader.GetGuid(reader.GetOrdinal("id_flujo_autenticacion")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            IdTenant = reader.GetInt64(reader.GetOrdinal("id_tenant")),
            IdEmpresa = reader.IsDBNull(reader.GetOrdinal("id_empresa")) ? null : reader.GetInt64(reader.GetOrdinal("id_empresa")),
            OtpHash = (byte[])reader["otp_hash"],
            OtpSalt = (byte[])reader["otp_salt"],
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            Intentos = reader.GetInt16(reader.GetOrdinal("intentos")),
            MaxIntentos = reader.GetInt16(reader.GetOrdinal("max_intentos"))
        };
    }

    public async Task<bool> IncrementarIntentoDesafioMfaAsync(Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpIncrementarIntentoMfa;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseInt(scalar) > 0;
    }

    public async Task<bool> MarcarDesafioMfaValidadoAsync(Guid idDesafioMfa, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpMarcarDesafioMfaValidado;
        command.Parameters.Add(CreateParameter("@id_desafio_mfa", SqlDbType.UniqueIdentifier, idDesafioMfa));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseInt(scalar) > 0;
    }

    public async Task<Guid> CrearSesionUsuarioAsync(
        Guid idSesionUsuario,
        long idUsuario,
        long idTenant,
        long idEmpresa,
        byte[] tokenHash,
        byte[]? refreshHash,
        string origenAutenticacion,
        bool mfaValidado,
        DateTime creadoUtc,
        DateTime expiraAbsolutaUtc,
        DateTime ultimaActividadUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? huellaDispositivo,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrearSesion;
        command.Parameters.Add(CreateParameter("@id_sesion_usuario", SqlDbType.UniqueIdentifier, idSesionUsuario));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));
        command.Parameters.Add(CreateParameter("@id_empresa", SqlDbType.BigInt, idEmpresa));
        command.Parameters.Add(CreateParameter("@token_hash", SqlDbType.Binary, tokenHash, 32));
        command.Parameters.Add(CreateParameter("@refresh_hash", SqlDbType.Binary, refreshHash, 32));
        command.Parameters.Add(CreateParameter("@origen_autenticacion", SqlDbType.VarChar, origenAutenticacion, 20));
        command.Parameters.Add(CreateParameter("@mfa_validado", SqlDbType.Bit, mfaValidado));
        command.Parameters.Add(CreateParameter("@creado_utc", SqlDbType.DateTime2, creadoUtc));
        command.Parameters.Add(CreateParameter("@expira_absoluta_utc", SqlDbType.DateTime2, expiraAbsolutaUtc));
        command.Parameters.Add(CreateParameter("@ultima_actividad_utc", SqlDbType.DateTime2, ultimaActividadUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, ipOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, agenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@huella_dispositivo", SqlDbType.NVarChar, huellaDispositivo, 200));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseGuid(scalar, idSesionUsuario);
    }

    public async Task<IReadOnlyList<string>> ObtenerPermisosUsuarioAsync(long idUsuario, long idTenant, CancellationToken cancellationToken)
    {
        var result = new List<string>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtenerPermisos;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            if (!reader.IsDBNull(reader.GetOrdinal("codigo")))
            {
                result.Add(reader.GetString(reader.GetOrdinal("codigo")));
            }
        }

        return result;
    }

    public async Task<IReadOnlyList<RecursoUiAccesoData>> ObtenerRecursosUiUsuarioAsync(long idUsuario, long idTenant, CancellationToken cancellationToken)
    {
        var result = new List<RecursoUiAccesoData>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtenerRecursosUi;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tenant", SqlDbType.BigInt, idTenant));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new RecursoUiAccesoData
            {
                IdRecursoUi = reader.GetInt64(reader.GetOrdinal("id_recurso_ui")),
                Codigo = reader.GetString(reader.GetOrdinal("codigo")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Ruta = reader.IsDBNull(reader.GetOrdinal("ruta")) ? null : reader.GetString(reader.GetOrdinal("ruta")),
                Componente = reader.IsDBNull(reader.GetOrdinal("componente")) ? null : reader.GetString(reader.GetOrdinal("componente")),
                Icono = reader.IsDBNull(reader.GetOrdinal("icono")) ? null : reader.GetString(reader.GetOrdinal("icono")),
                OrdenVisual = reader.GetInt32(reader.GetOrdinal("orden_visual")),
                IdRecursoUiPadre = reader.IsDBNull(reader.GetOrdinal("id_recurso_ui_padre")) ? null : reader.GetInt64(reader.GetOrdinal("id_recurso_ui_padre"))
            });
        }

        return result;
    }

    public async Task<Guid> CrearFlujoRestablecimientoClaveAsync(
        Guid idFlujoRestablecimientoClave,
        long idUsuario,
        short idTipoVerificacionRestablecimiento,
        DateTime expiraEnUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? solicitudId,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrearFlujoRestablecimiento;
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_tipo_verificacion_restablecimiento", SqlDbType.SmallInt, idTipoVerificacionRestablecimiento));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, expiraEnUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, ipOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, agenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, solicitudId, 64));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseGuid(scalar, idFlujoRestablecimientoClave);
    }

    public async Task<Guid> CrearTokenRestablecimientoClaveAsync(
        Guid idTokenRestablecimientoClave,
        long idUsuario,
        Guid idFlujoRestablecimientoClave,
        byte[] tokenHash,
        DateTime expiraEnUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? solicitudId,
        CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrearTokenRestablecimiento;
        command.Parameters.Add(CreateParameter("@id_token_restablecimiento_clave", SqlDbType.UniqueIdentifier, idTokenRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@token_hash", SqlDbType.Binary, tokenHash, 32));
        command.Parameters.Add(CreateParameter("@expira_en_utc", SqlDbType.DateTime2, expiraEnUtc));
        command.Parameters.Add(CreateParameter("@ip_origen", SqlDbType.NVarChar, ipOrigen, 45));
        command.Parameters.Add(CreateParameter("@agente_usuario", SqlDbType.NVarChar, agenteUsuario, 300));
        command.Parameters.Add(CreateParameter("@solicitud_id", SqlDbType.NVarChar, solicitudId, 64));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseGuid(scalar, idTokenRestablecimientoClave);
    }

    public async Task<TokenRestablecimientoData?> ObtenerTokenRestablecimientoPorHashAsync(byte[] tokenHash, Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtenerTokenRestablecimiento;
        command.Parameters.Add(CreateParameter("@token_hash", SqlDbType.Binary, tokenHash, 32));
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));

        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return null;
        }

        return new TokenRestablecimientoData
        {
            IdTokenRestablecimientoClave = reader.GetGuid(reader.GetOrdinal("id_token_restablecimiento_clave")),
            IdFlujoRestablecimientoClave = reader.IsDBNull(reader.GetOrdinal("id_flujo_restablecimiento_clave")) ? null : reader.GetGuid(reader.GetOrdinal("id_flujo_restablecimiento_clave")),
            IdUsuario = reader.GetInt64(reader.GetOrdinal("id_usuario")),
            ExpiraEnUtc = reader.GetDateTime(reader.GetOrdinal("expira_en_utc")),
            Usado = reader.GetBoolean(reader.GetOrdinal("usado")),
            FlujoUsado = reader.GetBoolean(reader.GetOrdinal("flujo_usado"))
        };
    }

    public async Task<bool> ConsumirTokenRestablecimientoAsync(Guid idTokenRestablecimientoClave, Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpConsumirTokenRestablecimiento;
        command.Parameters.Add(CreateParameter("@id_token_restablecimiento_clave", SqlDbType.UniqueIdentifier, idTokenRestablecimientoClave));
        command.Parameters.Add(CreateParameter("@id_flujo_restablecimiento_clave", SqlDbType.UniqueIdentifier, idFlujoRestablecimientoClave));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseInt(scalar) > 0;
    }

    public async Task<bool> ActualizarClaveUsuarioAsync(long idUsuario, byte[] hashClave, byte[] saltClave, string algoritmoClave, int iteracionesClave, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizarClaveUsuario;
        command.Parameters.Add(CreateParameter("@id_usuario", SqlDbType.BigInt, idUsuario));
        command.Parameters.Add(CreateParameter("@hash_clave", SqlDbType.VarBinary, hashClave, 128));
        command.Parameters.Add(CreateParameter("@salt_clave", SqlDbType.VarBinary, saltClave, 32));
        command.Parameters.Add(CreateParameter("@algoritmo_clave", SqlDbType.VarChar, algoritmoClave, 30));
        command.Parameters.Add(CreateParameter("@iteraciones_clave", SqlDbType.Int, iteracionesClave));

        var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return ParseInt(scalar) > 0;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }

    private static Guid ParseGuid(object? scalar, Guid fallback)
    {
        if (scalar is Guid guid)
        {
            return guid;
        }

        return Guid.TryParse(Convert.ToString(scalar), out var parsed) ? parsed : fallback;
    }

    private static int ParseInt(object? scalar)
    {
        if (scalar is null || scalar is DBNull)
        {
            return 0;
        }

        return Convert.ToInt32(scalar);
    }
}
