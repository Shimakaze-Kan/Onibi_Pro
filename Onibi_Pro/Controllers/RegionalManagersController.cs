using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
using Onibi_Pro.Application.RegionalManagers.Queries.GetCouriers;
using Onibi_Pro.Contracts.Orders;
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

        return Ok(_mapper.Map<IReadOnlyCollection<GetCouriersResponse>>(result));
    }
}
