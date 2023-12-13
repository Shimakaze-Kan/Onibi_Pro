namespace Onibi_Pro.Contracts.Restaurants;
public record CreateEmployeeResponse(Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string City,
    List<string> EmployeePositions);