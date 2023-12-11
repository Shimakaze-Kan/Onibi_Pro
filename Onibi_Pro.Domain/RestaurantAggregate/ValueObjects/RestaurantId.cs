using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
public sealed class RestaurantId : ValueObject
{
    public Guid Value { get; }

    private RestaurantId(Guid id)
    {
        Value = id;
    }

    public static RestaurantId Create(Guid id)
    {
        return new(id);
    }

    public static RestaurantId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
