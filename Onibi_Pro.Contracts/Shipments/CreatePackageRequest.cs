using static Onibi_Pro.Contracts.Shipments.CreatePackageRequest;

namespace Onibi_Pro.Contracts.Shipments;
public record CreatePackageRequest(IReadOnlyCollection<Ingredient> Ingredients, DateTime Until, bool IsUrgent, string Message)
{
    public record Ingredient(string Name, string Unit, decimal Quantity);
}
