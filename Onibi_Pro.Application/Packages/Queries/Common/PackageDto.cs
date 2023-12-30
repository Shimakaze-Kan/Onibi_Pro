using System.Text.Json;

namespace Onibi_Pro.Application.Packages.Queries.Common;
public class PackageDto
{
    public Guid PackageId { get; set; }
    public Guid? DestinationRestaurant { get; set; }
    public Guid? Manager { get; set; }
    public Guid? RegionalManager { get; set; }
    public Guid? SourceRestaurant { get; set; }
    public Guid? Courier { get; set; }
    public string Origin_Street { get; set; }
    public string Origin_City { get; set; }
    public string Origin_PostalCode { get; set; }
    public string Origin_Country { get; set; }
    public string Destination_Street { get; set; }
    public string Destination_City { get; set; }
    public string Destination_PostalCode { get; set; }
    public string Destination_Country { get; set; }
    public int Status { get; set; }
    public string Message { get; set; }
    public bool IsUrgent { get; set; }
    public string Ingredients { get; set; }
    public List<Ingredient> IngredientsList => JsonSerializer.Deserialize<List<Ingredient>>(Ingredients);
    public DateTime? Until { get; set; }
}

public class Ingredient
{
    public string Name { get; set; }
    public int Unit { get; set; }
    public decimal Quantity { get; set; }
}