using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Order
    {
        public static Error OrderNotFound => Error.NotFound(
            code: "Order.OrderNotFound", description: "Order with provided Id does not exist.");

        public static Error WrongMenuItemId => Error.Validation(
            code: "Order.WrongMenuItemId", description: "Provided menu item id is not supported.");

        public static Error QuantityGreaterThanZero => Error.Validation(
            code: "Order.QuantityMustBeGreaterThanZero", description: "Quantity must be greater than zero.");

        public static Error InvalidOrderItemAmount => Error.Validation(
            code: "Order.InvalidOrderItemAmount", description: "Order must contain at least one position.");

        public static Error RestaurantNotFound => Error.Validation(
            code: "Order.RestaurantNotFound", description: "Restaurant not found.");

        public static Error AlreadyCancelled => Error.Validation(
            code: "Order.AlreadyCancelled", description: "Order is already cancelled.");

        public static Error OperationForbidden => Error.Unauthorized(
            code: "Order.OperationForbidden", description: "Cannot cancel order.");
    }
}
