using MediatR;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetManagers;
public record GetManagersQuery(string RestaurantIdFilter,
    string ManagerIdFilter,
    string FirstNameFilter,
    string LastNameFilter,
    string EmailFilter) : IRequest<IReadOnlyCollection<ManagerDto>>;
