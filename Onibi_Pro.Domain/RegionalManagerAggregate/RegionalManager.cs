using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Domain.RegionalManagerAggregate;
public sealed class RegionalManager : AggregateRoot<RegionalManagerId>
{
    private readonly List<RestaurantId> _restaurants = [];
    private readonly List<Courier> _couriers = [];

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserId UserId { get; private set; }
    public IReadOnlyList<RestaurantId> Restaurants => _restaurants.ToList();
    public IReadOnlyList<Courier> Couriers => _couriers.ToList();

    private RegionalManager(RegionalManagerId id, UserId userId, string firstName,
        string lastName, List<RestaurantId>? restaurants)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        _restaurants = restaurants ?? [];
        UserId = userId;
    }

    public static RegionalManager Create(UserId userId, string firstName,
        string lastName, List<RestaurantId>? restaurants = null)
    {
        return new(RegionalManagerId.CreateUnique(), userId, firstName, lastName, restaurants);
    }

    public ErrorOr<Success> AssignRestaurant(RestaurantId restaurantId)
    {
        if (_restaurants.Contains(restaurantId))
        {
            return Errors.RegionalManager.RestaurantAlreadyAssigned;
        }

        _restaurants.Add(restaurantId);

        return new Success();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private RegionalManager() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
