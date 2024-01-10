namespace Onibi_Pro.Contracts.RegionalManagers;
public record GetManagersRequest(string RestaurantIdFilter = "",
    string ManagerIdFilter = "",
    string FirstNameFilter = "",
    string LastNameFilter = "",
    string EmailFilter = "");