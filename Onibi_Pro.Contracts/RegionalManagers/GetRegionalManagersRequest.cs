namespace Onibi_Pro.Contracts.RegionalManagers;
public record GetRegionalManagersRequest(int PageNumber = 1,
    int PageSize = 1,
    string RegionalManagerIdFilter = "",
    string FirstNameFilter = "",
    string LastNameFilter = "",
    string EmailFilter = "",
    string RestaurantIdFilter = "");