using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
public sealed class ManagerId : ValueObject
{
    public Guid Value { get; }

    private ManagerId(Guid id)
    {
        Value = id;
    }

    public static ManagerId Create(Guid id)
    {
        return new(id);
    }

    public static ManagerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
