using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.RestaurantAggregate;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
public record CreateRestaurantCommand(AddressObject Address, List<EmployeeCommand>? Employees) : IRequest<ErrorOr<Restaurant>>;

public record EmployeeCommand(string FirstName, string LastName, string Email, string City, List<EmployeePositionCommand> EmployeePositions);

public record EmployeePositionCommand(string Position);