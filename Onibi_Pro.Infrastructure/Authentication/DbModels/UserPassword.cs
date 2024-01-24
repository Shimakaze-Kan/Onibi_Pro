using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Authentication.DbModels;
public class UserPassword(UserId userId, string password)
{
    public UserId UserId { get; set; } = userId;
    public string Password { get; set; } = password;
}
