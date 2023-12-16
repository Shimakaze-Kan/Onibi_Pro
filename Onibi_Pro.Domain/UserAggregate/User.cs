using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.UserAggregate;
public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Password { get; }
    public UserTypes UserType { get; }

    private User(UserId userId, string firstName, string lastName,
        string email, string password, UserTypes userType)
        : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        UserType = userType;
    }

    public static User Create(UserId userId, string firstName, string lastName, string email,
        string password, UserTypes userType)
    {
        return new(userId, firstName, lastName, email, password, userType);
    }

    public static User CreateUnique(string firstName, string lastName, string email,
        string password, UserTypes userType)
    {
        return new(UserId.CreateUnique(), firstName, lastName, email, password, userType);
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