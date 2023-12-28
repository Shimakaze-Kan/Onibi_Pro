using static Onibi_Pro.Contracts.Orders.CreateOrderResponse;

namespace Onibi_Pro.Contracts.Orders;
public record CreateOrderResponse(Guid Id, DateTime DateTime, bool IsCancelled, IReadOnlyList<OrderItem> OrderItems)
{
    public record OrderItem(Guid MenuItemId, int Quantity);
};
