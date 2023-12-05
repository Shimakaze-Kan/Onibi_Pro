using Onibi_Pro.Domain.Models;

namespace Onibi_Pro.Domain.OrderAggregate.ValueObjects;
public sealed class OrderId : ValueObject
{
    public Guid Value { get; }

    private OrderId(Guid id)
    {
        Value = id;
    }

    public static OrderId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
