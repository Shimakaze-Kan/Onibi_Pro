using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
using Onibi_Pro.Application.Identity.Queries.GetRegionalManagerDetails;
using Onibi_Pro.Application.Identity.Queries.GetUsers;
using Onibi_Pro.Application.Identity.Queries.GetWhoami;
using Onibi_Pro.Contracts.Identity;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
public class IdentityController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("managerDetails/{userId}")]
    [ProducesResponseType(typeof(GetManagerDetailsResponse), 200)]
    public async Task<IActionResult> GetManagerDetails([FromRoute] Guid userId)
    {
        var query = new GetManagerDetailsQuery(UserId.Create(userId));

        var result = await _mediator.Send(query);

        return result.Match(result => Ok(_mapper.Map<GetManagerDetailsResponse>(result)), Problem);
    }
    
    [HttpGet("regionalManagerDetails/{userId}")]
    [ProducesResponseType(typeof(GetRegionalManagerDetailsResponse), 200)]
    [Authorize(Policy = AuthorizationPolicies.GlobalOrRegionalManagerAccess)]
    public async Task<IActionResult> GetRegionalManagerDetails([FromRoute] Guid userId)
    {
        var query = new GetRegionalManagerDetailsQuery(UserId.Create(userId));

        var result = await _mediator.Send(query);

        return result.Match(result => Ok(_mapper.Map<GetRegionalManagerDetailsResponse>(result)), Problem);
    }

    [HttpGet("whoami")]
    [ProducesResponseType(typeof(GetWhoamiResponse), 200)]
    public async Task<IActionResult> GetWhoami()
    {
        var result = await _mediator.Send(new GetWhoamiQuery());

        return result.Match(result => Ok(_mapper.Map<GetWhoamiResponse>(result)), Problem);
    }

    [HttpGet("users")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetUsersResponse>), 200)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetUsersQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(result => Ok(_mapper.Map<IReadOnlyCollection<GetUsersResponse>>(result)), Problem);
    }
}
