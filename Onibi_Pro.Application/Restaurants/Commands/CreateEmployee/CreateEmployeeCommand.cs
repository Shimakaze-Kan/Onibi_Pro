using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
public record CreateEmployeeCommand(Guid RestaurantId, string FirstName, string LastName, string Email,
        string City, List<string> EmployeePositions) : IRequest<ErrorOr<Employee>>;