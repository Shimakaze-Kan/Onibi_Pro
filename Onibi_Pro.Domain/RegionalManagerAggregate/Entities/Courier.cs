using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
public sealed class Courier : Entity<CourierId>
{
    public string Email { get; private set; }
    public string Phone { get; private set; }

    private Courier(CourierId id, string email, string phone)
        : base(id)
    {
        Email = email;
        Phone = phone;

    }

    public static Courier Create(CourierId id, string email, string phone)
    {
        return new(id, email, phone);
    }

    public static Courier CreateUnique(string email, string phone)
    {
        return new(CourierId.CreateUnique(), email, phone);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Courier() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
