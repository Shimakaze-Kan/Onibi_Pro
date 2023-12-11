using Onibi_Pro.Contracts.Common;

namespace Onibi_Pro.Contracts.Restaurants;

public record CreateRestaurantResponse(Guid Id, Address Address, List<Guid>? OrderIds, List<CreateRestaurantEmployeeResponse>? Employees,
    List<CreateRestaurantManagerResponse>? Managers);

public record CreateRestaurantEmployeeResponse(Guid Id, string FirstName, string LastName,
    string Email, string City, List<CreateRestaurantEmployeePositionResponse> EmployeePositions);

public record CreateRestaurantEmployeePositionResponse(string Position);

public record CreateRestaurantManagerResponse(Guid Id, string FirstName, string LastName, string Email);
