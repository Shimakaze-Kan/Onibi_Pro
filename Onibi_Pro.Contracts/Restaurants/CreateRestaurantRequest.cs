﻿using Onibi_Pro.Contracts.Common;

using static Onibi_Pro.Contracts.Restaurants.CreateRestaurantRequest;

namespace Onibi_Pro.Contracts.Restaurants;
public record CreateRestaurantRequest(Address Address, List<Employee>? Employees = null)
{
    public record Employee(string FirstName, string LastName, string Email,
        string City, List<EmployeePosition> EmployeePositions);

    public record EmployeePosition(string Position);
}
