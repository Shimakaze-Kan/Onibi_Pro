using static Onibi_Pro.Contracts.Orders.GetOrdersResponse;

namespace Onibi_Pro.Contracts.Orders;

public record GetOrdersResponse(IReadOnlyCollection<Order> Orders, long TotalCount)
{
    public record Order(Guid OrderId, DateTime OrderTime, bool IsCancelled,
        IReadOnlyList<OrderItem> OrderItems, decimal Total);

    public record OrderItem(Guid MenuItemId, int Quantity, string MenuItemName, decimal Price);
};
