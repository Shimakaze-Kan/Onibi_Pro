using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;

namespace Onibi_Pro.Domain.ShipmentAggregate.Entities;
public sealed class Package : Entity<PackageId>
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
    public ManagerId? RestaurantSourceManager { get; private set; } = null;
    public CourierId? Courier { get; private set; } = null;
    public Address? Origin { get; private set; } = null;
    public Address Destination { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public string Message { get; private set; }
    public bool IsUrgent { get; private set; }
    public IReadOnlyList<Ingredient> Ingredients => _ingredients.ToList();

    private Package(PackageId id,
        ManagerId manager,
        RegionalManagerId regionalManager,
        Address destination,
        ShipmentStatus shipmentStatus,
        string message,
        List<Ingredient> ingredients,
        bool isUrgent)
        : base(id)
    {
        Manager = manager;
        RegionalManager = regionalManager;
        Destination = destination;
        Status = shipmentStatus;
        Message = message;
        _ingredients = ingredients;
        IsUrgent = isUrgent;

    }

    public static Package Create(ManagerId manager,
        RegionalManagerId regionalManager,
        Address destination,
        string message,
        List<Ingredient> ingredients,
        bool isUrgent = false)
    {
        if (ingredients?.Any() != true)
        {
            throw new Exception();
        }

        return new(PackageId.CreateUnique(),
            manager,
            regionalManager,
            destination,
            ShipmentStatus.PendingRegionalManagerApproval,
            message,
            ingredients,
            isUrgent);
    }

    internal void AcceptShipmentAndAssignCourier(
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

    internal void AcceptShipmentFromRestaurantAndCourier(RegionalManagerId regionalManager,
        Address origin, ManagerId restaurantSourceManager, CourierId courier)
    {
        if (RegionalManager != regionalManager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRegionalManagerApproval)
        {
            Origin = origin;
            Courier = courier;
            RestaurantSourceManager = restaurantSourceManager;
            Status = ShipmentStatus.PendingRestaurantManagerApproval;
        }
    }

    internal void AcceptShipment(ManagerId manager)
    {
        if (RestaurantSourceManager! != manager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRestaurantManagerApproval)
        {
            Status = ShipmentStatus.AssignedToCourier;
        }
    }

    internal void RejectPackage(RegionalManagerId regionalManager)
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

    internal void RejectPackage(ManagerId manager)
    {
        if (RestaurantSourceManager! != manager)
        {
            return;
        }

        if (Status == ShipmentStatus.PendingRestaurantManagerApproval)
        {
            Status = ShipmentStatus.Rejected;
        }
    }

    internal void PickUp(CourierId courier)
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

    internal void ConfirmDelivery(ManagerId manager)
    {
        if (Manager != manager)
        {
            return;
        }

        if (Status == ShipmentStatus.CourierPickedUp)
        {
            Status = ShipmentStatus.Delivered;
        }
    }

    private bool CanRejectCurrentStatus(ShipmentStatus status)
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
