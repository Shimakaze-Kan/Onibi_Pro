using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
public record EmployeeDto(Guid Id, string FirstName, string LastName, string Email, string City, string Supervisors, List<string> Positions);
