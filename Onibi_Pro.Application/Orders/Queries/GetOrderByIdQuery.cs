using ErrorOr;

using MediatR;

namespace Onibi_Pro.Application.Orders.Queries;
public record GetOrderByIdQuery(Guid OrderId) : IRequest<ErrorOr<IReadOnlyCollection<OrderDto>>>;
