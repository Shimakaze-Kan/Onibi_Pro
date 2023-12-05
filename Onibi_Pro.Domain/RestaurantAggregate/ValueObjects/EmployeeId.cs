using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
public sealed class EmployeeId : ValueObject
{
    public Guid Value { get; }

    private EmployeeId(Guid id)
    {
        Value = id;
    }

    public static EmployeeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
