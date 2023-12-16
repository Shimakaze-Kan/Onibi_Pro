using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.UserAggregate.ValueObjects;
public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid id)
    {
        Value = id;
    }

    public static UserId Create(Guid id)
    {
        return new(id);
    }

    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
