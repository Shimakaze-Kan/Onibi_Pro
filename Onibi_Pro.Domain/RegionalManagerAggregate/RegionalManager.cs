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

    public UserId UserId { get; private set; }
    public IReadOnlyList<RestaurantId> Restaurants => _restaurants.ToList();
    public IReadOnlyList<Courier> Couriers => _couriers.ToList();

    private RegionalManager(RegionalManagerId id, UserId userId, List<RestaurantId>? restaurants)
        : base(id)
    {
        _restaurants = restaurants ?? [];
        UserId = userId;
    }

    public static ErrorOr<RegionalManager> Create(UserId userId, bool areRestaurantsAlreadyAssigned, List<RestaurantId>? restaurants = null)
    {
        if (areRestaurantsAlreadyAssigned)
        {
            return Errors.RegionalManager.RestaurantsAlreadyAssignedToOtherRegionalManagers;
        }

        return new RegionalManager(RegionalManagerId.CreateUnique(), userId, restaurants);
    }

    public ErrorOr<Success> AssignRestaurant(RestaurantId restaurantId, bool isRestaurantsAlreadyAssigned)
    {
        if (_restaurants.Contains(restaurantId))
        {
            return Errors.RegionalManager.RestaurantAlreadyAssigned;
        }

        if (isRestaurantsAlreadyAssigned)
        {
            return Errors.RegionalManager.RestaurantsAlreadyAssignedToOtherRegionalManagers;
        }

        _restaurants.Add(restaurantId);

        return new Success();
    }

    public ErrorOr<Success> AssignCourier(Courier courier, RegionalManagerId regionalManagerId)
    {
        if (regionalManagerId != Id)
        {
            return Errors.RegionalManager.WrongRegionalManager;
        }

        _couriers.Add(courier);

        return new Success();
    }

    public ErrorOr<Success> UnassignCourier(Courier courier, RegionalManagerId regionalManagerId)
    {
        if (regionalManagerId != Id)
        {
            return Errors.RegionalManager.WrongRegionalManager;
        }

        var indexOfCourier = _couriers.FindIndex(existingCourier => existingCourier == courier);
        if (indexOfCourier == -1)
        {
            return Errors.RegionalManager.CourierNotFound;
        }

        return new Success();
    }

    public bool HasCourier(CourierId courierId)
    {
        return _couriers.Any(courier => courier.Id == courierId);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private RegionalManager() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
