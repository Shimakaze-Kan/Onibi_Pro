using MediatR;

namespace Onibi_Pro.Application.Restaurants.Queries.GetEmployeeCities;
public record GetEmployeeCitiesQuery : IRequest<IReadOnlyCollection<string>>;
