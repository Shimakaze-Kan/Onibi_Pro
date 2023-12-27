namespace Onibi_Pro.Application.Orders.Queries.GetOrderById;
public record OrderPositionDto(string Name, int Quantity, DateTime OrderTime, bool IsCancelled);