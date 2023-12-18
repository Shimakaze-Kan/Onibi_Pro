namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface IPasswordService
{
    string HashPassword(string password, byte[]? predefinedSalt = null);
    bool VerifyPassword(string passwordToCompare, string hashedPassword);
}
