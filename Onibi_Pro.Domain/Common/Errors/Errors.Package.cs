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
    }
}
