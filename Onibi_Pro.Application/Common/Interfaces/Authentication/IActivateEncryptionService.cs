namespace Onibi_Pro.Application.Common.Interfaces.Authentication;
public interface IActivateEncryptionService
{
    (Guid UserId, string Email, bool IsExpired) DecryptData(string encryptedData);
    string EncryptData(Guid userId, string email);
}