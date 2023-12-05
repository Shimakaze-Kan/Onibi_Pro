using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
public sealed class EmployeePosition : ValueObject
{
    public Positions Position { get; }

    private EmployeePosition(Positions position)
    {
        Position = position;
    }

    public static EmployeePosition Create(Positions position)
    {
        return new(position);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Position;
    }

#pragma warning disable CS8618
    private EmployeePosition() { }
#pragma warning restore CS8618
}

public enum Positions
{
    Cashier,
    Cook,
    Server,
    Cleaner
}