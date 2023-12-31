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
    }
}
