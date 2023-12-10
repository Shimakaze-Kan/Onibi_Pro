using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Order
    {
        public static Error OrderNotFound => Error.NotFound(
            code: "Order.OrderNotFound", description: "Order with provided Id does not exist.");
    }
}
