﻿using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Orders.Queries;
using Onibi_Pro.Contracts.Orders;

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

    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetOrderByIdResponse>), 200)]
    public async Task<IActionResult> GetById(Guid orderId)
    {
        var query = new GetOrderByIdQuery(orderId);

        var result = await _mediator.Send(query);

        return result.Match(result
            => Ok(_mapper.Map<IReadOnlyCollection<GetOrderByIdResponse>>(result)), Problem);
    }
}
