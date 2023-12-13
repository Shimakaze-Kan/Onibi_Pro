namespace Onibi_Pro.Contracts.Restaurants;
public record GetEmployeeRequest(string FirstNameFilter = "",
    string LastNameFilter = "",
    string EmailFilter = "",
    string CityFilter = "",
    string PositionFilterList = "");
