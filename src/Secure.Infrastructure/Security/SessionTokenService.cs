using System.Security.Cryptography;
using System.Text;

namespace Secure.Platform.Infrastructure.Security;

/// <summary>
/// Implementacion para token opaco basado en GUID.
/// Autor: Mario Gomez.
/// </summary>
public sealed class SessionTokenService : ISessionTokenService
{
    /// <inheritdoc />
    public (Guid Token, byte[] TokenHash) GenerateOpaqueToken()
    {
        var token = Guid.NewGuid();
        var tokenBytes = Encoding.UTF8.GetBytes(token.ToString("N"));
        var tokenHash = SHA256.HashData(tokenBytes);
        return (token, tokenHash);
    }
}
