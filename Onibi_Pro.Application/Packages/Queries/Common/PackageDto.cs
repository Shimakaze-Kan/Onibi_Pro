using System.Text.Json;

using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate;

namespace Onibi_Pro.Application.Packages.Queries.Common;
public class PackageDto
{
    public Guid PackageId { get; set; }
    public Guid? DestinationRestaurant { get; set; }
    public Guid? Manager { get; set; }
    public Guid? RegionalManager { get; set; }
    public Guid? SourceRestaurant { get; set; }
    public Guid? Courier { get; set; }
    public string OriginStreet { get; set; } = "";
    public string OriginCity { get; set; } = "";
    public string OriginPostalCode { get; set; } = "";
    public string OriginCountry { get; set; } = "";
    public string DestinationStreet { get; set; } = "";
    public string DestinationCity { get; set; } = "";
    public string DestinationPostalCode { get; set; } = "";
    public string DestinationCountry { get; set; } = "";
    public ShipmentStatus Status { get; set; }
    public string Message { get; set; } = "";
    public bool IsUrgent { get; set; }
    public string Ingredients { get; set; } = "";

    public List<Ingredient> IngredientsList
        => JsonSerializer.Deserialize<List<IngredientJson>>(Ingredients)?
            .Select(ingredient => Ingredient.Create(ingredient.Name, ingredient.Unit, ingredient.Quantity)).ToList() ?? [];

    public string AvailableTransitions { get; set; } = "";

    public List<ShipmentStatus> AvailableTransitionsList
        => JsonSerializer.Deserialize<List<ShipmentStatus>>(string.IsNullOrEmpty(AvailableTransitions) ? "[]" : AvailableTransitions) ?? [];

    public DateTime? Until { get; set; }
}

public class IngredientJson
{
    public string Name { get; set; } = "";
    public UnitType Unit { get; set; }
    public decimal Quantity { get; set; }
}