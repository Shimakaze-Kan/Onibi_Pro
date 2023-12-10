using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Menu
    {
        public static Error InvalidAmountOfMenuItems => Error.Validation(
            code: "Menu.InvalidAmountOfMenuItems", description: "Menu must contain at leas one position.");

        public static Error InvalidName => Error.Validation(
            code: "Menu.InvalidName", description: "Menu name must be between 0 and 100 characters.");
    }
}
