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

        public static Error MenuItemNotFound => Error.NotFound(
            code: "Menu.MenuItemNotFound", description: "Menu item not found.");

        public static Error MenuNotFound => Error.NotFound(
            code: "Menu.MenuNotFound", description: "Menu not found.");
    }
}
