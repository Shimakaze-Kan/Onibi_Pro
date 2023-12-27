namespace Onibi_Pro.Application.Orders.Queries.GetOrders;
public record OrderDto(Guid OrderId, DateTime OrderTime, bool IsCancelled)
{
    public IReadOnlyList<OrderItemDto> OrderItems { get; init; } = [];
    public decimal Total { get; init; } = 0;
}

public record OrderItemDto(Guid MenuItemId, int Quantity, string MenuItemName, decimal Price);