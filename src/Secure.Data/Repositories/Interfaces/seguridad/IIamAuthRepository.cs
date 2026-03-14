using Secure.Platform.Data.Repositories.Models.Seguridad;

namespace Secure.Platform.Data.Repositories.Interfaces.Seguridad;

/// <summary>
/// Contrato de repositorio ADO.NET para flujo IAM de autenticacion.
/// Autor: Mario Gomez.
/// </summary>
public interface IIamAuthRepository
{
    Task<UsuarioAutenticacionData?> ObtenerUsuarioParaAutenticacionAsync(string tenantCodigo, string identificador, CancellationToken cancellationToken);

    Task<IReadOnlyList<EmpresaAccesoData>> ObtenerEmpresasOperablesUsuarioAsync(long idUsuario, long idTenant, CancellationToken cancellationToken);

    Task<Guid> CrearFlujoAutenticacionAsync(
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
        CancellationToken cancellationToken);

    Task<bool> MarcarFlujoAutenticacionUsadoAsync(Guid idFlujoAutenticacion, bool mfaValidado, CancellationToken cancellationToken);

    Task<bool> MarcarFlujoAutenticacionMfaValidadoAsync(Guid idFlujoAutenticacion, CancellationToken cancellationToken);

    Task<FlujoAutenticacionData?> ObtenerFlujoAutenticacionAsync(Guid idFlujoAutenticacion, CancellationToken cancellationToken);

    Task<Guid> CrearDesafioMfaAsync(
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
        CancellationToken cancellationToken);

    Task<DesafioMfaData?> ObtenerDesafioMfaAsync(Guid idDesafioMfa, CancellationToken cancellationToken);

    Task<bool> IncrementarIntentoDesafioMfaAsync(Guid idDesafioMfa, CancellationToken cancellationToken);

    Task<bool> MarcarDesafioMfaValidadoAsync(Guid idDesafioMfa, CancellationToken cancellationToken);

    Task<Guid> CrearSesionUsuarioAsync(
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
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> ObtenerPermisosUsuarioAsync(long idUsuario, long idTenant, long idEmpresa, CancellationToken cancellationToken);

    Task<IReadOnlyList<RecursoUiAccesoData>> ObtenerRecursosUiUsuarioAsync(long idUsuario, long idTenant, long idEmpresa, CancellationToken cancellationToken);

    Task<SesionTokenData?> ObtenerSesionPorTokenHashAsync(byte[] tokenHash, bool actualizarActividadUtc, CancellationToken cancellationToken);

    Task<Guid> CrearFlujoRestablecimientoClaveAsync(
        Guid idFlujoRestablecimientoClave,
        long idUsuario,
        short idTipoVerificacionRestablecimiento,
        DateTime expiraEnUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? solicitudId,
        CancellationToken cancellationToken);

    Task<Guid> CrearTokenRestablecimientoClaveAsync(
        Guid idTokenRestablecimientoClave,
        long idUsuario,
        Guid idFlujoRestablecimientoClave,
        byte[] tokenHash,
        DateTime expiraEnUtc,
        string? ipOrigen,
        string? agenteUsuario,
        string? solicitudId,
        CancellationToken cancellationToken);

    Task<TokenRestablecimientoData?> ObtenerTokenRestablecimientoPorHashAsync(byte[] tokenHash, Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken);

    Task<bool> ConsumirTokenRestablecimientoAsync(Guid idTokenRestablecimientoClave, Guid idFlujoRestablecimientoClave, CancellationToken cancellationToken);

    Task<bool> ActualizarClaveUsuarioAsync(long idUsuario, byte[] hashClave, byte[] saltClave, string algoritmoClave, int iteracionesClave, CancellationToken cancellationToken);
}
