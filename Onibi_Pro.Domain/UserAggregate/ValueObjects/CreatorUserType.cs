using Onibi_Pro.Domain.Common.Models;

namespace Onibi_Pro.Domain.UserAggregate.ValueObjects;
public sealed class CreatorUserType : ValueObject
{
    public UserTypes Value { get; }

    private CreatorUserType(UserTypes userType)
    {
        Value = userType;
    }

    public static CreatorUserType Create(UserTypes userType)
    {
        return new(userType);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
