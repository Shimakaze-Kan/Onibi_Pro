using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RegionalManagerAggregate;
public sealed class RegionalManager : AggregateRoot<RegionalManagerId>
{
    private readonly List<RestaurantId> _restaurants;

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public IReadOnlyList<RestaurantId> Restaurants => _restaurants.ToList();

    private RegionalManager(RegionalManagerId id, string firstName,
        string lastName, List<RestaurantId>? restaurants)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        _restaurants = restaurants ?? [];
    }

    public static RegionalManager Create(RegionalManagerId id, string firstName,
        string lastName, List<RestaurantId>? restaurants = null)
    {
        return new(id, firstName, lastName, restaurants);
    }

    public static RegionalManager CreateUnique(string firstName,
        string lastName, List<RestaurantId>? restaurants = null)
    {
        return new(RegionalManagerId.CreateUnique(), firstName, lastName, restaurants);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private RegionalManager() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
