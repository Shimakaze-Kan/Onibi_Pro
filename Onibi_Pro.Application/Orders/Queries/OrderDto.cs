namespace Onibi_Pro.Application.Orders.Queries;
public record OrderDto(string Name, int Quantity, DateTime OrderTime, bool IsCancelled);