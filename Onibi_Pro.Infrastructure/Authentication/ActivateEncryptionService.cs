using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Options;

using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Infrastructure.Authentication.Configurations;

namespace Onibi_Pro.Infrastructure.Authentication;

internal sealed class ActivateEncryptionService : IActivateEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    private readonly int _expirationInMinutes;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ActivateEncryptionService(IOptions<ActivateEncryptionConfiguration> options,
        IDateTimeProvider dateTimeProvider)
    {
        _iv = Encoding.UTF8.GetBytes(options.Value.InitializationVector);
        _key = Encoding.UTF8.GetBytes(options.Value.EncryptionKey);
        _expirationInMinutes = options.Value.ExpirationInMinutes;
        _dateTimeProvider = dateTimeProvider;
    }

    public string EncryptData(Guid userId, string email)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = _key;
        aesAlg.IV = _iv;

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            var expirationTime = _dateTimeProvider.UtcNow.AddMinutes(_expirationInMinutes);
            var jsonData = JsonSerializer.Serialize<ActivateLinkDto>(
                new() { Email = email, ExpirationTime = expirationTime, UserId = userId });
            swEncrypt.Write(jsonData);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public (Guid UserId, string Email, bool IsExpired) DecryptData(string encryptedData)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = _key;
        aesAlg.IV = _iv;

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedData));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        var decryptedText = srDecrypt.ReadToEnd();
        var obj = JsonSerializer.Deserialize<ActivateLinkDto>(decryptedText);

        if (obj is null)
        {
            throw new InvalidOperationException("Decrypted data does not contain a valid userId, email and expiration time.");
        }

        var isExpired = _dateTimeProvider.UtcNow > obj.ExpirationTime;

        return (obj.UserId, obj.Email, isExpired);
    }

    private class ActivateLinkDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = "";
        public DateTime ExpirationTime { get; set; }
    }
}
