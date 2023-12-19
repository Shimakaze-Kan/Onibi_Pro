using Onibi_Pro.Domain.Common.Models;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.ShipmentAggregate.Entities;
using Onibi_Pro.Domain.ShipmentAggregate.ValueObjects;

namespace Onibi_Pro.Domain.ShipmentAggregate;
public sealed class Shipment : AggregateRoot<ShipmentId>
{
    private readonly List<Package> _packages;
    private readonly List<Courier> _couriers;

    public IReadOnlyList<Package> Packages => _packages.ToList();
    public IReadOnlyList<Courier> Couriers => _couriers.ToList();

    private Shipment(ShipmentId id, List<Package>? packages, List<Courier>? couriers)
        : base(id)
    {
        _packages = packages ?? [];
        _couriers = couriers ?? [];
    }

    public static Shipment Create(List<Package>? packages = null, List<Courier>? couriers = null)
    {
        return new(ShipmentId.CreateUnique(), packages, couriers);
    }

    public void AcceptShipmentAndAssignCourier(PackageId package,
        RegionalManagerId regionalManager,
        Address origin,
        CourierId courier)
    {
        var foundPackage = GetPackageOrDefault(package);
        var foundCourier = GetCourierOrDefault(courier);

        if (foundPackage is null || foundCourier is null)
        {
            return;
        }

        foundPackage.AcceptShipmentAndAssignCourier(regionalManager, origin, courier);
    }

    public void AcceptShipmentFromRestaurantAndCourier(PackageId package,
        RegionalManagerId regionalManager,
        Address origin,
        ManagerId restaurantSourceManager,
        CourierId courier)
    {
        var foundPackage = GetPackageOrDefault(package);
        var foundCourier = GetCourierOrDefault(courier);

        if (foundPackage is null || foundCourier is null)
        {
            return;
        }

        foundPackage.AcceptShipmentFromRestaurantAndCourier(regionalManager,
            origin,
            restaurantSourceManager,
            courier);
    }

    public void AcceptShipment(PackageId package, ManagerId manager)
    {
        var foundPackage = GetPackageOrDefault(package);

        if (foundPackage is null)
        {
            return;
        }

        foundPackage.AcceptShipment(manager);
    }

    public void RejectPackage(PackageId package, RegionalManagerId regionalManager)
    {
        var foundPackage = GetPackageOrDefault(package);

        if (foundPackage is null)
        {
            return;
        }

        foundPackage.RejectPackage(regionalManager);
    }

    public void RejectPackage(PackageId package, ManagerId manager)
    {
        var foundPackage = GetPackageOrDefault(package);

        if (foundPackage is null)
        {
            return;
        }

        foundPackage.RejectPackage(manager);
    }

    public void PickUp(PackageId package, CourierId courier)
    {
        var foundPackage = GetPackageOrDefault(package);
        var foundCourier = GetCourierOrDefault(courier);

        if (foundPackage is null || foundCourier is null)
        {
            return;
        }

        foundPackage.PickUp(courier);
    }

    public void ConfirmDelivery(PackageId package, ManagerId manager)
    {
        var foundPackage = GetPackageOrDefault(package);

        if (foundPackage is null)
        {
            return;
        }

        foundPackage.ConfirmDelivery(manager);
    }

    public void RegisterNewCourier(Courier courier)
    {
        var foundCourier = GetCourierOrDefault(courier.Id);

        if (foundCourier is not null)
        {
            return;
        }

        _couriers.Add(courier);
    }

    public void UnregisterCourier(Courier courier)
    {
        var foundCourier = GetCourierOrDefault(courier.Id);

        if (foundCourier is null)
        {
            return;
        }

        _couriers.Remove(courier);
    }

    public void RegisterNewPackage(Package package)
    {
        var foundPackage = GetPackageOrDefault(package.Id);

        if (foundPackage is not null)
        {
            return;
        }

        _packages.Add(package);
    }

    public void UnegisterNewPackage(Package package)
    {
        var foundPackage = GetPackageOrDefault(package.Id);

        if (foundPackage is null)
        {
            return;
        }

        _packages.Remove(package);
    }

    private Package? GetPackageOrDefault(PackageId package)
    {
        return _packages.SingleOrDefault(x => x.Id == package);
    }

    private Courier? GetCourierOrDefault(CourierId courier)
    {
        return _couriers.SingleOrDefault(x => x.Id == courier);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Shipment() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
