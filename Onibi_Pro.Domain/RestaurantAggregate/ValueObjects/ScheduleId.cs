using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
public class ScheduleId : ValueObject
{
    public Guid Value { get; }

    private ScheduleId(Guid id)
    {
        Value = id;
    }

    public static ScheduleId Create(Guid id)
    {
        return new(id);
    }

    public static ScheduleId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private ScheduleId() { }
}
