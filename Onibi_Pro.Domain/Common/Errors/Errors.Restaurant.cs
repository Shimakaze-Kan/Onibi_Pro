using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Restaurant
    {
        public static Error InvalidOrderId => Error.Validation(
            code: "Restaurant.InvalidOrderId", description: "Order Id is invalid.");
        
        public static Error RestaurantNotFound => Error.Validation(
            code: "Restaurant.NotFound", description: "Restaurant not found.");
    }
}
