using System.Security.Cryptography;
using System.Text;

using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Infrastructure.Authentication;
internal sealed class PasswordService : IPasswordService
{
    private const int KeySize = 64;
    private const int Iterations = 350000;
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public string HashPassword(string password, byte[]? predefinedSalt = null)
    {
        byte[] salt;

        if (predefinedSalt is not null)
        {
            salt = predefinedSalt;
        }
        else
        {
            salt = RandomNumberGenerator.GetBytes(KeySize);
        }
        byte[] hash = GenerateHash(password, salt);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string passwordToCompare, string hashedPassword)
    {
        var hashedPasswordParts = hashedPassword.Split('.');
        var salt = Convert.FromBase64String(hashedPasswordParts[0]);
        var hash = Convert.FromBase64String(hashedPasswordParts[1]);
        var hashToCompare = GenerateHash(passwordToCompare, salt);

        return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
    }

    private byte[] GenerateHash(string password, byte[] salt)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            _hashAlgorithm,
            KeySize);
    }
}
