using System.Collections.Generic;

using Azure;

using ErrorOr;

using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
using Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
using Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
using Onibi_Pro.Application.RegionalManagers.Queries.GetCouriers;
using Onibi_Pro.Application.RegionalManagers.Queries.GetManagers;
using Onibi_Pro.Application.RegionalManagers.Queries.GetRegionalManagers;
using Onibi_Pro.Contracts.RegionalManagers;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RegionalManagersController : ApiBaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RegionalManagersController(IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("courier")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    public async Task<IActionResult> CreateCourier([FromBody] CreateCourierRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCourierCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(result => Ok(), Problem);
    }

    [HttpGet("courier")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetCouriersResponse>), 200)]
    public async Task<IActionResult> GetCouriers(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCouriersQuery(), cancellationToken);

        return result.Match(result => Ok(_mapper.Map<IReadOnlyCollection<GetCouriersResponse>>(result)), Problem);
    }

    [HttpGet("manager")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetManagersResponse>), 200)]
    public async Task<IActionResult> GetManagers([FromQuery] GetManagersRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetManagersQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(result => Ok(_mapper.Map<IReadOnlyCollection<GetManagersResponse>>(result)), Problem);
    }

    [HttpPost("manager")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    public async Task<IActionResult> CreateManager(CreateManagerRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateManagerCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPut("manager")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    public async Task<IActionResult> UpdateManager(UpdateManagerRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateManagerCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.GlobalManagerAccess)]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetRegionalManagerResponse>), 200)]
    public async Task<IActionResult> GetRegionalManagers(GetRegionalManagersRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetRegionalManagersQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(result => Ok(_mapper.Map<IReadOnlyCollection<GetRegionalManagerResponse>>(result)), Problem);
    }
}
