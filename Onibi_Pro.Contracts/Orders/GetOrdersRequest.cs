namespace Onibi_Pro.Contracts.Orders;

public record GetOrdersRequest(int StartRow = 1, int Amount = 20);