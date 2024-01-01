﻿using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.UserAggregate.Events;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.UserAggregate;
public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public UserTypes UserType { get; private set; }

    private User(UserId userId, string firstName, string lastName,
        string email, UserTypes userType)
        : base(userId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserType = userType;
    }

    public static ErrorOr<User> Create(string firstName, string lastName,
        string email, string hashedPassword, CreatorUserType currentCreatorType)
    {
        ErrorOr<UserTypes> userType = currentCreatorType.Value switch
        {
            UserTypes.GlobalManager => UserTypes.RegionalManager,
            UserTypes.RegionalManager => UserTypes.Manager,
            _ => Errors.User.UnsupportedCreatorUserType
        };

        if (userType.IsError)
        {
            return userType.Errors;
        }

        var user = new User(UserId.CreateUnique(), firstName, lastName, email, userType.Value);
        user.AddDomainEvent(new UserCreated(user.Id, user.Email, hashedPassword));

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