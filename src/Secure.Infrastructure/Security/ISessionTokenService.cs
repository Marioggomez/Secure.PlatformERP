namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Servicio para generar token opaco con hash SHA256.
/// Autor: Mario Gomez.
/// </summary>
public interface ISessionTokenService
{
    /// <summary>
    /// Genera token GUID y su hash SHA256 para persistir.
    /// </summary>
    (Guid Token, byte[] TokenHash) GenerateOpaqueToken();
}
