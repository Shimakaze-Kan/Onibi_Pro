using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.GlobalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;

namespace Onibi_Pro.Domain.GlobalManagerAggregate;
public sealed class GlobalManager : AggregateRoot<GlobalManagerId>
{
    private readonly List<RegionalManagerId> _regionalManagers;

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public IReadOnlyList<RegionalManagerId> RegionalManagers => _regionalManagers.ToList();

    private GlobalManager(GlobalManagerId id, string firstName,
        string lastName, List<RegionalManagerId>? regionalManagers)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        _regionalManagers = regionalManagers ?? new();
    }

    public static GlobalManager Create(string firstName, string lastName,
        List<RegionalManagerId>? regionalManagers = null)
    {
        return new(GlobalManagerId.CreateUnique(), firstName, lastName, regionalManagers);
    }
}
