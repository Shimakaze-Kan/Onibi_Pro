using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.UserAggregate.Events;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.UserAggregate;
public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public UserTypes UserType { get; }

    private User(UserId userId, string firstName, string lastName,
        string email, UserTypes userType)
        : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserType = userType;
    }

    public static User Create(UserId userId, string firstName,
        string lastName, string email, UserTypes userType)
    {
        return new(userId, firstName, lastName, email, userType);
    }

    public static User CreateUnique(string firstName, string lastName,
        string email, string hashedPassword, UserTypes userType)
    {
        var user = new User(UserId.CreateUnique(), firstName, lastName, email, userType);
        user.AddDomainEvent(new UserCreated(user.Id, hashedPassword));

        return user;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

public enum UserTypes
{
    Manager,
    RegionalManager,
    GlobalManager
}