using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployeeCities;
public record GetEmployeeCitiesQuery : IRequest<ErrorOr<IReadOnlyCollection<string>>>;
