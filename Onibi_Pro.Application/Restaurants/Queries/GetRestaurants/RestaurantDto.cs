namespace Onibi_Pro.Application.Restaurants.Queries.GetRestaurants;
public record RestaurantDto
{
    public Guid Id { get; init; }
    public string Street { get; init; }
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Country { get; init; }
}