using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
public sealed class Courier : Entity<CourierId>
{
    public UserId UserId { get; private set; }
    public string Phone { get; private set; }

    private Courier(CourierId id, UserId userId, string phone)
        : base(id)
    {
        Phone = phone;
        UserId = userId;
    }

    public static ErrorOr<Courier> CreateUnique(UserId userId, string phone, UserTypes userType)
    {
        if (userType != UserTypes.Courier)
        {
            return Errors.RegionalManager.WrongUserCourierType;
        }

        if (string.IsNullOrEmpty(phone) && phone is { Length: > 10 })
        {
            return Errors.RegionalManager.InvalidPhoneNumber;
        }

        return new Courier(CourierId.CreateUnique(), userId, phone);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Courier() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
