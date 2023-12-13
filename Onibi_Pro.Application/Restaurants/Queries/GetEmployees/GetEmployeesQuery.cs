using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
public record GetEmployeesQuery(Guid RestaurantId,
    string FirstNameFilter,
    string LastNameFilter,
    string EmailFilter,
    string CityFilter,
    List<string>? PositionFilter) : IRequest<ErrorOr<IReadOnlyCollection<EmployeeDto>>>;
