using static Onibi_Pro.Contracts.Orders.CreateOrderRequest;

namespace Onibi_Pro.Contracts.Orders;
public record CreateOrderRequest(List<OrderItem> OrderItems)
{
    public record OrderItem(int Quantity, Guid MenuItemId);
};
