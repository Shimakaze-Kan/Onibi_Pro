using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.MenuAggregate.ValueObjects;
public sealed class MenuId : ValueObject
{
    public Guid Value { get; }

    private MenuId(Guid id)
    {
        Value = id;
    }

    public static MenuId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    public static MenuId Create(Guid id)
    {
        return new(id);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
