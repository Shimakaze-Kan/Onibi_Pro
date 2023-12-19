using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RestaurantAggregate.Entities;
public sealed class Manager : Entity<ManagerId>
{
    public UserId UserId { get; }

    private Manager(ManagerId id, UserId userId)
        : base(id)
    {
        UserId = userId;
    }

    public static Manager Create(UserId userId)
    {
        return new(ManagerId.CreateUnique(), userId);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Manager() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
