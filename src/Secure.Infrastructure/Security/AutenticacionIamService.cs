using System.Security.Cryptography;
using System.Text;
using Secure.Platform.Contracts.Dtos.Seguridad;
using Secure.Platform.Data.Repositories.Interfaces.Seguridad;
using Secure.Platform.Data.Repositories.Models.Seguridad;

namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Implementacion del flujo IAM de autenticacion enterprise.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AutenticacionIamService : IAutenticacionIamService
{
    private const short PropositoMfaLogin = 1;
    private const short CanalNotificacionCorreo = 1;
    private const int MinutosExpiracionFlujoLogin = 10;
    private const int MinutosExpiracionOtp = 5;
    private const int MinutosExpiracionRestablecimiento = 30;
    private const int HorasExpiracionSesion = 8;
    private const short MaxIntentosOtp = 5;

    private readonly IIamAuthRepository _repository;
    private readonly ISessionTokenService _sessionTokenService;

    public AutenticacionIamService(IIamAuthRepository repository, ISessionTokenService sessionTokenService)
    {
        _repository = repository;
        _sessionTokenService = sessionTokenService;
    }

    public async Task<LoginResponseDto> IniciarLoginAsync(LoginRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TenantCodigo) || string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Contrasena))
        {
            return new LoginResponseDto
            {
                Autenticado = false,
                RequiereMfa = false,
                Mensaje = "Tenant, usuario y contrasena son obligatorios."
            };
        }

        var usuario = await _repository.ObtenerUsuarioParaAutenticacionAsync(
            request.TenantCodigo.Trim(),
            request.Usuario.Trim(),
            cancellationToken).ConfigureAwait(false);

        if (usuario is null)
        {
            return LoginFallido();
        }

        if (!usuario.ActivoUsuario || !usuario.ActivoCredencial || usuario.IdEstadoUsuario != 1)
        {
            return LoginFallido();
        }

        if (!ValidarContrasena(request.Contrasena, usuario))
        {
            return LoginFallido();
        }

        if (!usuario.IdEmpresa.HasValue)
        {
            return new LoginResponseDto
            {
                Autenticado = false,
                RequiereMfa = false,
                Mensaje = "El usuario no tiene empresa activa asignada para operar en el ERP."
            };
        }

        var idFlujoAutenticacion = Guid.NewGuid();
        await _repository.CrearFlujoAutenticacionAsync(
            idFlujoAutenticacion,
            usuario.IdUsuario,
            usuario.IdTenant,
            usuario.MfaHabilitado,
            false,
            DateTime.UtcNow.AddMinutes(MinutosExpiracionFlujoLogin),
            request.IpOrigen,
            request.AgenteUsuario,
            request.HuellaDispositivo,
            request.SolicitudId,
            cancellationToken).ConfigureAwait(false);

        if (usuario.MfaHabilitado)
        {
            var codigoOtp = GenerarOtpNumerico(6);
            var otpSalt = RandomNumberGenerator.GetBytes(16);
            var otpHash = CalcularHashOtp(codigoOtp, otpSalt);
            var idDesafioMfa = Guid.NewGuid();

            await _repository.CrearDesafioMfaAsync(
                idDesafioMfa,
                usuario.IdUsuario,
                usuario.IdTenant,
                usuario.IdEmpresa,
                idFlujoAutenticacion,
                PropositoMfaLogin,
                CanalNotificacionCorreo,
                "LOGIN",
                otpHash,
                otpSalt,
                DateTime.UtcNow.AddMinutes(MinutosExpiracionOtp),
                MaxIntentosOtp,
                cancellationToken).ConfigureAwait(false);

            return new LoginResponseDto
            {
                Autenticado = true,
                RequiereMfa = true,
                Mensaje = "Credenciales validadas. Se requiere MFA para completar la autenticacion.",
                IdFlujoAutenticacion = idFlujoAutenticacion,
                IdDesafioMfa = idDesafioMfa,
                CodigoMfaPrueba = codigoOtp,
                IdUsuario = usuario.IdUsuario,
                IdTenant = usuario.IdTenant,
                IdEmpresa = usuario.IdEmpresa,
                UsuarioMostrar = usuario.NombreMostrar
            };
        }

        var sesion = await CrearSesionAsync(
            usuario.IdUsuario,
            usuario.IdTenant,
            usuario.IdEmpresa.Value,
            request.IpOrigen,
            request.AgenteUsuario,
            request.HuellaDispositivo,
            false,
            cancellationToken).ConfigureAwait(false);

        await _repository.MarcarFlujoAutenticacionUsadoAsync(idFlujoAutenticacion, false, cancellationToken).ConfigureAwait(false);

        var permisos = await _repository.ObtenerPermisosUsuarioAsync(usuario.IdUsuario, usuario.IdTenant, cancellationToken).ConfigureAwait(false);
        var recursos = await _repository.ObtenerRecursosUiUsuarioAsync(usuario.IdUsuario, usuario.IdTenant, cancellationToken).ConfigureAwait(false);

        return new LoginResponseDto
        {
            Autenticado = true,
            RequiereMfa = false,
            Mensaje = "Autenticacion exitosa.",
            IdFlujoAutenticacion = idFlujoAutenticacion,
            TokenSesion = sesion.TokenPlano,
            ExpiraSesionUtc = sesion.ExpiraEnUtc,
            IdUsuario = usuario.IdUsuario,
            IdTenant = usuario.IdTenant,
            IdEmpresa = usuario.IdEmpresa,
            UsuarioMostrar = usuario.NombreMostrar,
            Permisos = permisos,
            RecursosUi = recursos.Select(MapearRecursoUi).ToList()
        };
    }

    public async Task<ValidarMfaResponseDto> ValidarMfaAsync(ValidarMfaRequestDto request, CancellationToken cancellationToken)
    {
        if (request.IdFlujoAutenticacion == Guid.Empty || request.IdDesafioMfa == Guid.Empty || string.IsNullOrWhiteSpace(request.CodigoOtp))
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "Debe proporcionar flujo, desafio y codigo OTP."
            };
        }

        var desafio = await _repository.ObtenerDesafioMfaAsync(request.IdDesafioMfa, cancellationToken).ConfigureAwait(false);
        if (desafio is null || !desafio.IdFlujoAutenticacion.HasValue || desafio.IdFlujoAutenticacion.Value != request.IdFlujoAutenticacion)
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "El desafio MFA no existe o no pertenece al flujo indicado."
            };
        }

        if (desafio.Usado)
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "El desafio MFA ya fue utilizado."
            };
        }

        if (DateTime.UtcNow > desafio.ExpiraEnUtc)
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "El codigo MFA expiro. Solicite uno nuevo."
            };
        }

        if (desafio.Intentos >= desafio.MaxIntentos)
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "Se alcanzo el maximo de intentos MFA. Solicite un nuevo codigo."
            };
        }

        var otpHash = CalcularHashOtp(request.CodigoOtp.Trim(), desafio.OtpSalt);
        if (!CryptographicOperations.FixedTimeEquals(otpHash, desafio.OtpHash))
        {
            await _repository.IncrementarIntentoDesafioMfaAsync(request.IdDesafioMfa, cancellationToken).ConfigureAwait(false);
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "Codigo MFA invalido."
            };
        }

        if (!desafio.IdEmpresa.HasValue)
        {
            return new ValidarMfaResponseDto
            {
                Validado = false,
                Mensaje = "No existe empresa activa asociada al desafio MFA."
            };
        }

        await _repository.MarcarDesafioMfaValidadoAsync(request.IdDesafioMfa, cancellationToken).ConfigureAwait(false);
        await _repository.MarcarFlujoAutenticacionUsadoAsync(request.IdFlujoAutenticacion, true, cancellationToken).ConfigureAwait(false);

        var sesion = await CrearSesionAsync(
            desafio.IdUsuario,
            desafio.IdTenant,
            desafio.IdEmpresa.Value,
            request.IpOrigen,
            request.AgenteUsuario,
            request.HuellaDispositivo,
            true,
            cancellationToken).ConfigureAwait(false);

        var permisos = await _repository.ObtenerPermisosUsuarioAsync(desafio.IdUsuario, desafio.IdTenant, cancellationToken).ConfigureAwait(false);
        var recursos = await _repository.ObtenerRecursosUiUsuarioAsync(desafio.IdUsuario, desafio.IdTenant, cancellationToken).ConfigureAwait(false);

        return new ValidarMfaResponseDto
        {
            Validado = true,
            Mensaje = "MFA validado correctamente.",
            TokenSesion = sesion.TokenPlano,
            ExpiraSesionUtc = sesion.ExpiraEnUtc,
            IdUsuario = desafio.IdUsuario,
            IdTenant = desafio.IdTenant,
            IdEmpresa = desafio.IdEmpresa,
            UsuarioMostrar = null,
            Permisos = permisos,
            RecursosUi = recursos.Select(MapearRecursoUi).ToList()
        };
    }

    public async Task<ReenviarMfaResponseDto> ReenviarMfaAsync(ReenviarMfaRequestDto request, CancellationToken cancellationToken)
    {
        if (request.IdFlujoAutenticacion == Guid.Empty || request.IdDesafioMfa == Guid.Empty)
        {
            return new ReenviarMfaResponseDto
            {
                Reenviado = false,
                Mensaje = "Debe indicar flujo y desafio MFA."
            };
        }

        var desafioActual = await _repository.ObtenerDesafioMfaAsync(request.IdDesafioMfa, cancellationToken).ConfigureAwait(false);
        if (desafioActual is null || !desafioActual.IdFlujoAutenticacion.HasValue || desafioActual.IdFlujoAutenticacion.Value != request.IdFlujoAutenticacion)
        {
            return new ReenviarMfaResponseDto
            {
                Reenviado = false,
                Mensaje = "El desafio MFA ya no esta disponible para reenvio."
            };
        }

        var codigoOtp = GenerarOtpNumerico(6);
        var otpSalt = RandomNumberGenerator.GetBytes(16);
        var otpHash = CalcularHashOtp(codigoOtp, otpSalt);
        var nuevoDesafioId = Guid.NewGuid();

        await _repository.CrearDesafioMfaAsync(
            nuevoDesafioId,
            desafioActual.IdUsuario,
            desafioActual.IdTenant,
            desafioActual.IdEmpresa,
            request.IdFlujoAutenticacion,
            PropositoMfaLogin,
            CanalNotificacionCorreo,
            "LOGIN-REENVIO",
            otpHash,
            otpSalt,
            DateTime.UtcNow.AddMinutes(MinutosExpiracionOtp),
            MaxIntentosOtp,
            cancellationToken).ConfigureAwait(false);

        return new ReenviarMfaResponseDto
        {
            Reenviado = true,
            Mensaje = "Se genero un nuevo desafio MFA.",
            IdDesafioMfa = nuevoDesafioId,
            ExpiraEnUtc = DateTime.UtcNow.AddMinutes(MinutosExpiracionOtp),
            CodigoMfaPrueba = codigoOtp
        };
    }

    public async Task<IniciarRestablecimientoClaveResponseDto> IniciarRestablecimientoAsync(IniciarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.TenantCodigo) || string.IsNullOrWhiteSpace(request.UsuarioOCorreo))
        {
            return new IniciarRestablecimientoClaveResponseDto
            {
                Iniciado = false,
                Mensaje = "Tenant y usuario/correo son obligatorios."
            };
        }

        var usuario = await _repository.ObtenerUsuarioParaAutenticacionAsync(
            request.TenantCodigo.Trim(),
            request.UsuarioOCorreo.Trim(),
            cancellationToken).ConfigureAwait(false);

        if (usuario is null)
        {
            return new IniciarRestablecimientoClaveResponseDto
            {
                Iniciado = true,
                Mensaje = "Si el usuario existe, recibira instrucciones de restablecimiento."
            };
        }

        var idFlujo = Guid.NewGuid();
        var expiraEn = DateTime.UtcNow.AddMinutes(MinutosExpiracionRestablecimiento);
        await _repository.CrearFlujoRestablecimientoClaveAsync(
            idFlujo,
            usuario.IdUsuario,
            request.IdTipoVerificacionRestablecimiento,
            expiraEn,
            request.IpOrigen,
            request.AgenteUsuario,
            request.SolicitudId,
            cancellationToken).ConfigureAwait(false);

        var tokenPlano = Guid.NewGuid();
        var tokenHash = CalcularHashToken(tokenPlano);
        var idToken = Guid.NewGuid();

        await _repository.CrearTokenRestablecimientoClaveAsync(
            idToken,
            usuario.IdUsuario,
            idFlujo,
            tokenHash,
            expiraEn,
            request.IpOrigen,
            request.AgenteUsuario,
            request.SolicitudId,
            cancellationToken).ConfigureAwait(false);

        return new IniciarRestablecimientoClaveResponseDto
        {
            Iniciado = true,
            Mensaje = "Flujo de restablecimiento iniciado.",
            IdFlujoRestablecimientoClave = idFlujo,
            TokenRestablecimientoPrueba = tokenPlano.ToString("N"),
            ExpiraEnUtc = expiraEn
        };
    }

    public async Task<CompletarRestablecimientoClaveResponseDto> CompletarRestablecimientoAsync(CompletarRestablecimientoClaveRequestDto request, CancellationToken cancellationToken)
    {
        if (request.IdFlujoRestablecimientoClave == Guid.Empty || string.IsNullOrWhiteSpace(request.TokenRestablecimiento) || string.IsNullOrWhiteSpace(request.NuevaContrasena))
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "Debe completar flujo, token y nueva contrasena."
            };
        }

        if (request.NuevaContrasena.Trim().Length < 8)
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "La nueva contrasena debe tener al menos 8 caracteres."
            };
        }

        if (!Guid.TryParse(request.TokenRestablecimiento.Trim(), out var tokenGuid))
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "El token de restablecimiento no es valido."
            };
        }

        var tokenHash = CalcularHashToken(tokenGuid);
        var token = await _repository.ObtenerTokenRestablecimientoPorHashAsync(
            tokenHash,
            request.IdFlujoRestablecimientoClave,
            cancellationToken).ConfigureAwait(false);

        if (token is null)
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "El token de restablecimiento no existe para este flujo."
            };
        }

        if (token.Usado || token.FlujoUsado)
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "El token ya fue utilizado."
            };
        }

        if (DateTime.UtcNow > token.ExpiraEnUtc)
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "El token de restablecimiento expiro."
            };
        }

        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = CalcularHashContrasenaSha512(request.NuevaContrasena.Trim(), salt);

        var actualizada = await _repository.ActualizarClaveUsuarioAsync(
            token.IdUsuario,
            hash,
            salt,
            "SHA2_512",
            100000,
            cancellationToken).ConfigureAwait(false);

        if (!actualizada)
        {
            return new CompletarRestablecimientoClaveResponseDto
            {
                Restablecido = false,
                Mensaje = "No fue posible actualizar la credencial del usuario."
            };
        }

        await _repository.ConsumirTokenRestablecimientoAsync(
            token.IdTokenRestablecimientoClave,
            request.IdFlujoRestablecimientoClave,
            cancellationToken).ConfigureAwait(false);

        return new CompletarRestablecimientoClaveResponseDto
        {
            Restablecido = true,
            Mensaje = "Contrasena restablecida correctamente."
        };
    }

    private async Task<(string TokenPlano, DateTime ExpiraEnUtc)> CrearSesionAsync(
        long idUsuario,
        long idTenant,
        long idEmpresa,
        string? ipOrigen,
        string? agenteUsuario,
        string? huellaDispositivo,
        bool mfaValidado,
        CancellationToken cancellationToken)
    {
        var (token, tokenHash) = _sessionTokenService.GenerateOpaqueToken();
        var ahora = DateTime.UtcNow;
        var expira = ahora.AddHours(HorasExpiracionSesion);

        await _repository.CrearSesionUsuarioAsync(
            Guid.NewGuid(),
            idUsuario,
            idTenant,
            idEmpresa,
            tokenHash,
            null,
            "LOCAL",
            mfaValidado,
            ahora,
            expira,
            ahora,
            ipOrigen,
            agenteUsuario,
            huellaDispositivo,
            cancellationToken).ConfigureAwait(false);

        return (token.ToString("N"), expira);
    }

    private static LoginResponseDto LoginFallido()
    {
        return new LoginResponseDto
        {
            Autenticado = false,
            RequiereMfa = false,
            Mensaje = "Credenciales invalidas o usuario inactivo."
        };
    }

    private static bool ValidarContrasena(string contrasena, UsuarioAutenticacionData usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.AlgoritmoClave))
        {
            return false;
        }

        var algoritmo = usuario.AlgoritmoClave.Trim().ToUpperInvariant();

        if (algoritmo == "SHA2_512")
        {
            var hashCalculado = CalcularHashContrasenaSha512(contrasena, usuario.SaltClave);
            return CryptographicOperations.FixedTimeEquals(hashCalculado, usuario.HashClave);
        }

        if (algoritmo.StartsWith("PBKDF2", StringComparison.Ordinal))
        {
            var longitud = usuario.HashClave.Length == 0 ? 64 : usuario.HashClave.Length;
            var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(contrasena),
                usuario.SaltClave,
                usuario.IteracionesClave > 0 ? usuario.IteracionesClave : 100000,
                HashAlgorithmName.SHA512,
                longitud);
            return CryptographicOperations.FixedTimeEquals(hashCalculado, usuario.HashClave);
        }

        return false;
    }

    private static byte[] CalcularHashContrasenaSha512(string contrasena, byte[] salt)
    {
        var passBytes = Encoding.UTF8.GetBytes(contrasena);
        var payload = new byte[salt.Length + passBytes.Length];
        Buffer.BlockCopy(salt, 0, payload, 0, salt.Length);
        Buffer.BlockCopy(passBytes, 0, payload, salt.Length, passBytes.Length);
        return SHA512.HashData(payload);
    }

    private static string GenerarOtpNumerico(int longitud)
    {
        var buffer = new StringBuilder(longitud);
        for (var i = 0; i < longitud; i++)
        {
            buffer.Append(RandomNumberGenerator.GetInt32(0, 10));
        }

        return buffer.ToString();
    }

    private static byte[] CalcularHashOtp(string otp, byte[] salt)
    {
        var otpBytes = Encoding.UTF8.GetBytes(otp);
        var payload = new byte[salt.Length + otpBytes.Length];
        Buffer.BlockCopy(salt, 0, payload, 0, salt.Length);
        Buffer.BlockCopy(otpBytes, 0, payload, salt.Length, otpBytes.Length);
        return SHA256.HashData(payload);
    }

    private static byte[] CalcularHashToken(Guid token)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(token.ToString("N")));
    }

    private static RecursoUiAccesoDto MapearRecursoUi(RecursoUiAccesoData model)
    {
        return new RecursoUiAccesoDto
        {
            IdRecursoUi = model.IdRecursoUi,
            Codigo = model.Codigo,
            Nombre = model.Nombre,
            Ruta = model.Ruta,
            Componente = model.Componente,
            Icono = model.Icono,
            OrdenVisual = model.OrdenVisual,
            IdRecursoUiPadre = model.IdRecursoUiPadre
        };
    }
}
