using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
public record EditEmployeeCommand(Guid RestaurantId, Guid EmployeeId, string FirstName, string LastName, string Email,
        string City, List<string> EmployeePositions) : IRequest<ErrorOr<Success>>;