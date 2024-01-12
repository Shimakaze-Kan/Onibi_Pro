using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Orders.Commands.CancelOrder;
using Onibi_Pro.Application.Orders.Commands.CreateOrder;
using Onibi_Pro.Application.Orders.Queries.GetOrderById;
using Onibi_Pro.Application.Orders.Queries.GetOrders;
using Onibi_Pro.Contracts.Orders;
using Onibi_Pro.Domain.OrderAggregate.ValueObjects;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OrdersController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("id/{orderId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetOrderByIdResponse>), 200)]
    public async Task<IActionResult> GetById([FromRoute] Guid orderId)
    {
        var query = new GetOrderByIdQuery(orderId);

        var result = await _mediator.Send(query);

        return result.Match(result
            => Ok(_mapper.Map<IReadOnlyCollection<GetOrderByIdResponse>>(result)), Problem);
    }

    [HttpPost("{restaurantId}")]
    [ProducesResponseType(typeof(CreateOrderResponse), 200)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, [FromRoute] Guid restaurantId)
    {
        var command = _mapper.Map<CreateOrderCommand>((request, restaurantId));

        var result = await _mediator.Send(command);

        return result.Match(result
            => Ok(_mapper.Map<CreateOrderResponse>(result)), Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetOrdersResponse), 200)]
    public async Task<IActionResult> GetOrders([FromQuery] GetOrdersRequest request)
    {
        var query = _mapper.Map<GetOrdersQuery>(request);

        var result = await _mediator.Send(query);

        return result.Match(result => Ok(_mapper.Map<GetOrdersResponse>(result)), Problem);
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> CancelOrder([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelOrderCommand(OrderId.Create(orderId)), cancellationToken);

        return result.Match(result => Ok(), Problem);
    }
}
