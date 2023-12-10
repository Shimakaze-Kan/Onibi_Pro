using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
public sealed class RegionalManagerId : ValueObject
{
    public Guid Value { get; }

    public RegionalManagerId(Guid id)
    {
        Value = id;
    }

    public static RegionalManagerId Create(Guid id)
    {
        return new(id);
    }

    public static RegionalManagerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
