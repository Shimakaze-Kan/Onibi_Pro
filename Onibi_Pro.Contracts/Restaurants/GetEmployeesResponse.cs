namespace Onibi_Pro.Contracts.Restaurants;
public record GetEmployeesResponse(Guid Id, string FirstName, string LastName, string Email, string City, string Supervisors, List<string> Positions);