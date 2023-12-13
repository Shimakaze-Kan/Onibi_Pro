namespace Onibi_Pro.Contracts.Restaurants;
public record CreateEmployeeRequest(string FirstName, string LastName, string Email,
        string City, List<string> EmployeePositions);