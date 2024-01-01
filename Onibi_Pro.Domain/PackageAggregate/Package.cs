using ErrorOr;

using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Domain.PackageAggregate;
public sealed class Package : AggregateRoot<PackageId>
{
    private readonly List<Ingredient> _ingredients;

    private readonly IReadOnlyCollection<ShipmentStateTransition> _transitions = new List<ShipmentStateTransition>
    {
        new(ShipmentStatus.PendingRegionalManagerApproval, ShipmentStatus.AssignedToCourier,
            (package) => package.Courier is not null && package.Origin is not null),
        new(ShipmentStatus.PendingRegionalManagerApproval, ShipmentStatus.Rejected),
        new(ShipmentStatus.AssignedToCourier, ShipmentStatus.CourierPickedUp),
        new(ShipmentStatus.AssignedToCourier, ShipmentStatus.Rejected),
        new(ShipmentStatus.CourierPickedUp, ShipmentStatus.Delivered),
        new(ShipmentStatus.CourierPickedUp, ShipmentStatus.Rejected),

        new(ShipmentStatus.PendingRegionalManagerApproval, ShipmentStatus.PendingRestaurantManagerApproval,
            (package) => package.SourceRestaurant is not null && package.Courier is not null && package.Origin is not null),
        new(ShipmentStatus.PendingRestaurantManagerApproval, ShipmentStatus.AssignedToCourier,
            (package) => package.Courier is not null && package.Origin is not null),
        new(ShipmentStatus.PendingRestaurantManagerApproval, ShipmentStatus.Rejected),
    };

    public ManagerId Manager { get; private set; }
    public RegionalManagerId RegionalManager { get; private set; }
    public RestaurantId? SourceRestaurant { get; private set; } = null;
    public CourierId? Courier { get; private set; } = null;
    public Address? Origin { get; private set; } = null;
    public Address Destination { get; private set; }
    public RestaurantId DestinationRestaurant { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public string Message { get; private set; }
    public bool IsUrgent { get; private set; }
    public DateTime Until { get; private set; }
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.ToList();
    public IReadOnlyList<ShipmentStatus> AvailableTransitions
    {
        get
        {
            return _transitions.Where(transition => transition.StartState == Status)
                .Select(transition => transition.EndState).ToList();
        }
        private set { } // required by ef core
    }

    private Package(PackageId id,
        ManagerId manager,
        RegionalManagerId regionalManager,
        Address destination,
        RestaurantId destinationRestaurant,
        ShipmentStatus shipmentStatus,
        string message,
        List<Ingredient> ingredients,
        bool isUrgent,
        DateTime until)
        : base(id)
    {
        Manager = manager;
        RegionalManager = regionalManager;
        Destination = destination;
        Status = shipmentStatus;
        Message = message;
        _ingredients = ingredients;
        IsUrgent = isUrgent;
        DestinationRestaurant = destinationRestaurant;
        Until = until;
    }

    public static ErrorOr<Package> Create(ManagerId manager,
        RegionalManagerId regionalManager,
        Address destination,
        RestaurantId destinationRestaurant,
        string message,
        List<Ingredient> ingredients,
        DateTime until,
        bool isUrgent = false)
    {
        if (ingredients?.Any() != true)
        {
            return Errors.Package.WrongIngredientAmount;
        }

        return new Package(PackageId.CreateUnique(),
            manager,
            regionalManager,
            destination,
            destinationRestaurant,
            ShipmentStatus.PendingRegionalManagerApproval,
            message,
            ingredients,
            isUrgent,
            until);
    }

    public ErrorOr<Success> AssignCourier(RegionalManagerId regionalManager, CourierId courier)
    {
        if (RegionalManager != regionalManager)
        {
            return Errors.Package.WrongRegionalManager;
        }

        Courier = courier;
        // fire event

        return new Success();
    }

    public ErrorOr<Success> AcceptShipmentByRegionalManager(RegionalManagerId regionalManager, Address origin)
    {
        if (RegionalManager != regionalManager)
        {
            return Errors.Package.WrongRegionalManager;
        }

        Origin = origin;

        if (!TryChangeStatus(ShipmentStatus.AssignedToCourier))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    public ErrorOr<Success> AcceptShipmentFromRestaurant(RegionalManagerId regionalManager,
        Address origin, RestaurantId sourceRestaurant)
    {
        if (RegionalManager != regionalManager)
        {
            return Errors.Package.WrongRegionalManager;
        }

        if (sourceRestaurant == DestinationRestaurant)
        {
            return Errors.Package.WrongSourceRestaurant;
        }

        Origin = origin;
        SourceRestaurant = sourceRestaurant;

        if (!TryChangeStatus(ShipmentStatus.PendingRestaurantManagerApproval))
        {
            return Errors.Package.StatusChangeFailed;
        }

        //fire event
        return new Success();
    }

    public ErrorOr<Success> AcceptShipmentBySourceManager(RestaurantId sourceManagerRestaurant)
    {
        if (SourceRestaurant! != sourceManagerRestaurant)
        {
            return Errors.Package.WrongSourceRestaurantManager;
        }

        if (!TryChangeStatus(ShipmentStatus.AssignedToCourier))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    public ErrorOr<Success> RejectShipmentByRegionalManager(RegionalManagerId regionalManager)
    {
        if (RegionalManager != regionalManager)
        {
            return Errors.Package.WrongRegionalManager;
        }

        if (!TryChangeStatus(ShipmentStatus.Rejected))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    public ErrorOr<Success> RejectPackageBySourceRestaurantManager(RestaurantId sourceManagerRestaurant)
    {
        if (SourceRestaurant! != sourceManagerRestaurant)
        {
            return Errors.Package.WrongSourceRestaurantManager;
        }

        if (!TryChangeStatus(ShipmentStatus.Rejected))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    public ErrorOr<Success> PickUp(CourierId courier)
    {
        if (Courier! != courier)
        {
            return Errors.Package.WrongCourier;
        }

        if (!TryChangeStatus(ShipmentStatus.CourierPickedUp))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    public ErrorOr<Success> ConfirmDelivery(RestaurantId destinationRestaurantId)
    {
        if (DestinationRestaurant != destinationRestaurantId)
        {
            return Errors.Package.WrongDestinationRestaurant;
        }

        if (!TryChangeStatus(ShipmentStatus.Delivered))
        {
            return Errors.Package.StatusChangeFailed;
        }

        return new Success();
    }

    private bool TryChangeStatus(ShipmentStatus newStatus)
    {
        var transition = _transitions.SingleOrDefault(t => t.StartState == Status && t.EndState == newStatus);
        if (transition?.CanTransition(this) ?? false)
        {
            Status = newStatus;
            return true;
        }

        return false;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Package() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private class ShipmentStateTransition
    {
        public ShipmentStatus StartState { get; }
        public ShipmentStatus EndState { get; }
        public Func<Package, bool> Condition { get; }

        public ShipmentStateTransition(ShipmentStatus startState, ShipmentStatus endState, Func<Package, bool> condition = null!)
        {
            StartState = startState;
            EndState = endState;
            Condition = condition ?? ((_) => true);
        }

        public bool CanTransition(Package package)
        {
            return Condition(package);
        }
    }
}

public enum ShipmentStatus
{
    PendingRegionalManagerApproval,
    PendingRestaurantManagerApproval,
    AssignedToCourier,
    CourierPickedUp,
    Delivered,
    Rejected
}
