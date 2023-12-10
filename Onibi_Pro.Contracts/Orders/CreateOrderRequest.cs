namespace Onibi_Pro.Contracts.Orders;
public record CreateOrderRequest(List<OrderItemRequest> OrderItems);

public record OrderItemRequest(int Quantity, Guid MenuItemId);