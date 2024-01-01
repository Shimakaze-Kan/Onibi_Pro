using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Package
    {
        public static Error PackageNotFound => Error.NotFound(
            code: "Package.PackageNotFound", description: "Package with provided Id does not exist.");

        public static Error WrongIngredientAmount => Error.Validation(
            code: "Package.WrongIngredientAmount", description: "At least one ingredient is required to place an order.");

        public static Error WrongRegionalManager => Error.Validation(
            code: "Package.WrongRegionalManager", description: "This regional manager cannot perform this operation.");

        public static Error WrongSourceRestaurantManager => Error.Validation(
            code: "Package.WrongSourceRestaurantManager", description: "This manager cannot perform this operation.");

        public static Error WrongCourier => Error.Validation(
            code: "Package.WrongCourier", description: "This courier cannot perform this operation.");

        public static Error WrongDestinationRestaurant => Error.Validation(
            code: "Package.WrongDestinationRestaurant", description: "This is not correct destination restaurant for this package.");

        public static Error WrongSourceRestaurant => Error.Validation(
            code: "Package.WrongSourceRestaurant", description: "The source and destination restaurants cannot be the same.");

        public static Error StatusChangeFailed => Error.Validation(
            code: "Package.StatusChangeFailed", description: "The conditions for setting the shipment status have not been met.");
    }
}
