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
    private readonly IReadOnlyCollection<ShipmentStatus> _canRejectStatuses = new List<ShipmentStatus>()
    {
        ShipmentStatus.PendingRegionalManagerApproval,
        ShipmentStatus.PendingRestaurantManagerApproval,
        ShipmentStatus.AssignedToCourier,
        ShipmentStatus.CourierPickedUp
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

    public void AcceptShipmentAndAssignCourier(
        RegionalManagerId regionalManager, Address origin, CourierId courier)
    {
        if (RegionalManager != regionalManager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRegionalManagerApproval)
        {
            Origin = origin;
            Courier = courier;
            Status = ShipmentStatus.AssignedToCourier;
        }
    }

    public void AcceptShipmentFromRestaurantAndCourier(RegionalManagerId regionalManager,
        Address origin, RestaurantId sourceRestaurant, CourierId courier)
    {
        if (RegionalManager != regionalManager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRegionalManagerApproval)
        {
            Origin = origin;
            Courier = courier;
            SourceRestaurant = sourceRestaurant;
            Status = ShipmentStatus.PendingRestaurantManagerApproval;
        }
    }

    public void AcceptShipment(ManagerId manager)
    {
        if (SourceRestaurant! != manager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRestaurantManagerApproval)
        {
            Status = ShipmentStatus.AssignedToCourier;
        }
    }

    public void RejectPackage(RegionalManagerId regionalManager)
    {
        if (RegionalManager != regionalManager)
        {
            return;
        }

        if (CanRejectCurrentStatus(Status))
        {
            Status = ShipmentStatus.Rejected;
        }
    }

    public void RejectPackage(ManagerId manager)
    {
        if (SourceRestaurant! != manager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRestaurantManagerApproval)
        {
            Status = ShipmentStatus.Rejected;
        }
    }

    public void PickUp(CourierId courier)
    {
        if (Courier! != courier)
        {
            return;
        }

        if (Status == ShipmentStatus.AssignedToCourier)
        {
            Status = ShipmentStatus.CourierPickedUp;
        }
    }

    public void ConfirmDelivery(RestaurantId restaurantId)
    {
        if (DestinationRestaurant != restaurantId)
        {
            return;
        }

        if (Status == ShipmentStatus.CourierPickedUp)
        {
            Status = ShipmentStatus.Delivered;
        }
    }

    public bool CanRejectCurrentStatus(ShipmentStatus status)
    {
        return _canRejectStatuses.Contains(status);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Package() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
