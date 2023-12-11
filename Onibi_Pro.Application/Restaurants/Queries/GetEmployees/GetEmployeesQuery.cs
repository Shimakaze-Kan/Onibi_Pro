using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
public record GetEmployeesQuery(Guid RestaurantId) : IRequest<ErrorOr<IReadOnlyCollection<EmployeeDto>>>;
