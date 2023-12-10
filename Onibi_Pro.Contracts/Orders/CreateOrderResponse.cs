namespace Onibi_Pro.Contracts.Orders;
public record CreateOrderResponse(Guid Id, DateTime DateTime, bool IsCancelled, IReadOnlyList<OrderItemResponse> OrderItems);

public record OrderItemResponse(Guid MenuItemId, int Quantity);
