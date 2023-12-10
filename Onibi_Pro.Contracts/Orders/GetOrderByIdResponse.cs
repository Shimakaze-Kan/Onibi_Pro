namespace Onibi_Pro.Contracts.Orders;
public record GetOrderByIdResponse(string Name, int Quantity, DateTime OrderTime, bool IsCancelled);
