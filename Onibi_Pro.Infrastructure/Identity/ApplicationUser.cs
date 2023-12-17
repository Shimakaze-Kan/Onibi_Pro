using Microsoft.AspNetCore.Identity;

using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Infrastructure.Identity;
public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserTypes UserType { get; private set; }

    public ApplicationUser(User user)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
        UserType = user.UserType;
        Email = user.Email;
        UserName = user.Email;
        Id = user.Id.Value;
    }

    public User ToDomainUser()
    {
        return User.Create(UserId.Create(Id),
            FirstName,
            LastName,
            Email,
            UserType);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationUser() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
