using ErrorOr;

using MediatR;

using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Commands.CancelOrder;
public record CancelOrderCommand(OrderId OrderId) : IRequest<ErrorOr<Success>>;
