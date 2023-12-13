namespace Onibi_Pro.Contracts.Restaurants;

public record EditEmployeeRequest(Guid EmployeeId, string FirstName, string LastName, string Email,
        string City, List<string> EmployeePositions);