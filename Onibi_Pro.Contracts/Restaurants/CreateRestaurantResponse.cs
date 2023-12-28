using Onibi_Pro.Contracts.Common;

using static Onibi_Pro.Contracts.Restaurants.CreateRestaurantResponse;

namespace Onibi_Pro.Contracts.Restaurants;

public record CreateRestaurantResponse(Guid Id, Address Address, List<Guid>? OrderIds, List<Employee>? Employees,
    List<Manager>? Managers)
{
    public record Employee(Guid Id, string FirstName, string LastName,
        string Email, string City, List<EmployeePosition> EmployeePositions);

    public record EmployeePosition(string Position);

    public record Manager(Guid Id, string FirstName, string LastName, string Email);
};
