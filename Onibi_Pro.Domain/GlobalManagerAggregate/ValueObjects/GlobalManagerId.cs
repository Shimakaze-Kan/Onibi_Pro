using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.GlobalManagerAggregate.ValueObjects;
public sealed class GlobalManagerId : ValueObject
{
    public Guid Value { get; }

    private GlobalManagerId(Guid id)
    {
        Value = id;
    }

    public static GlobalManagerId Create(Guid id)
    {
        return new(id);
    }

    public static GlobalManagerId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
