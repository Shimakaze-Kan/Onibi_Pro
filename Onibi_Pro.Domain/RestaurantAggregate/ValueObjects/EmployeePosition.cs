using Onibi_Pro.Domain.Models;

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
}

public enum Positions
{
    Cashier,
    Cook,
    Server,
    Cleaner
}