using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;
public sealed class ShipmentId : ValueObject
{
    public Guid Value { get; }

    private ShipmentId(Guid id)
    {
        Value = id;
    }

    public static ShipmentId Create(Guid id)
    {
        return new(id);
    }

    public static ShipmentId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
