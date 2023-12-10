namespace Onibi_Pro.Application.Orders.Queries.GetOrderById;
public record OrderDto(string Name, int Quantity, DateTime OrderTime, bool IsCancelled);