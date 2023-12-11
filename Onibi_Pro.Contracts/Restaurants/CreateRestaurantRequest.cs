using Onibi_Pro.Contracts.Common;

namespace Onibi_Pro.Contracts.Restaurants;
public record CreateRestaurantRequest(Address Address, List<Guid>? OrderIds = null, List<EmployeeRequest>? Employees = null,
    List<ManagerRequest>? Managers = null);

public record EmployeeRequest(string FirstName, string LastName, string Email, string City, List<EmployeePositionRequest> EmployeePositions);

public record EmployeePositionRequest(string Position);

public record ManagerRequest(string FirstName, string LastName, string Email);