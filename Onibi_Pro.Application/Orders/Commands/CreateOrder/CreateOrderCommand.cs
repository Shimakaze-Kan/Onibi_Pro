using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.OrderAggregate;

namespace Onibi_Pro.Application.Orders.Commands.CreateOrder;
public record CreateOrderCommand(List<OrderItemComand> OrderItems) : IRequest<ErrorOr<Order>>;

public record OrderItemComand(int Quantity, Guid MenuItemId);
