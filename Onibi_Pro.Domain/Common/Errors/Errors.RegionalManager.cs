using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class RegionalManager
    {
        public static Error RestaurantAlreadyAssigned => Error.Validation(
            code: "RegionalManager.RestaurantAlreadyAssigned", description: "This restaurant is already assigned to this regional manager.");

        public static Error RegionalManagerNotFound => Error.NotFound(
            code: "RegionalManager.NotFound", description: "Regional manager not found.");

        public static Error CourierNotFound => Error.NotFound(
            code: "RegionalManager.CourierNotFound", description: "Courier not found.");

        public static Error WrongRegionalManager => Error.Validation(
            code: "RegionalManager.WrongRegionalManager", description: "Only regional manager can assign a courier to themselves.");

        public static Error WrongUserCourierType => Error.Validation(
            code: "Courier.WrongUserCourierType", description: "User must be of the courier type in order to be created.");
        
        public static Error InvalidPhoneNumber => Error.Validation(
            code: "Courier.InvalidPhoneNumber", description: "Phone number is invalid.");
        
        public static Error RestaurantsAlreadyAssignedToOtherRegionalManagers => Error.Conflict(
            code: "RegionalManager.RestaurantsAlreadyAssigned", description: "One of the restaurants listed is already assigned to another regional manager.");
    }
}
