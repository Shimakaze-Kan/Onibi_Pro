using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
public record GetRegionalManagersQuery(
    int PageNumber,
    int PageSize,
    string RegionalManagerIdFilter,
    string FirstNameFilter,
    string LastNameFilter,
    string EmailFilter,
    string RestaurantIdFilter) : IRequest<ErrorOr<RegionalManagerDto>>;