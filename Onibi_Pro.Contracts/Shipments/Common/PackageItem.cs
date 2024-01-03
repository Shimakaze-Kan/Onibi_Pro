using Onibi_Pro.Contracts.Common;

using static Onibi_Pro.Contracts.Shipments.Common.PackageItem;

namespace Onibi_Pro.Contracts.Shipments.Common;
public record PackageItem(
    Guid PackageId,
    Guid? DestinationRestaurant,
    Guid? Manager,
    Guid? RegionalManager,
    Guid? SourceRestaurant,
    Guid? Courier,
    Address? OriginAddress,
    Address DestinationAddress,
    string Status,
    string Message,
    bool IsUrgent,
    string? CourierPhone,
    List<Ingredient> Ingredients,
    List<string> AvailableTransitions,
    DateTime? Until)
{
    public record Ingredient(string Name, string Unit, decimal Quantity);
}