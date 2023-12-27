namespace Onibi_Pro.Contracts.Orders;

public record GetOrdersResponse(Guid OrderId, DateTime OrderTime, bool IsCancelled, IReadOnlyList<OrderItemDtoResponse> OrderItems, decimal Total);

public record OrderItemDtoResponse(Guid MenuItemId, int Quantity, string MenuItemName, decimal Price);