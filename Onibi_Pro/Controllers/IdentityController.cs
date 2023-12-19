using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
using Onibi_Pro.Application.Identity.Queries.GetWhoami;
using Onibi_Pro.Contracts.Identity;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

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

    [HttpGet("managerDetails/{managerId}")]
    [ProducesResponseType(typeof(GetManagerDetailsResponse), 200)]
    public async Task<IActionResult> GetManagerDetails([FromRoute] Guid managerId)
    {
        var query = new GetManagerDetailsQuery(UserId.Create(managerId));

        var result = await _mediator.Send(query);

        return Ok(_mapper.Map<GetManagerDetailsResponse>(result));
    }

    [HttpGet("whoami")]
    [ProducesResponseType(typeof(GetWhoamiResponse), 200)]
    public async Task<IActionResult> GetWhoami()
    {
        var result = await _mediator.Send(new GetWhoamiQuery());

        return Ok(_mapper.Map<GetWhoamiResponse>(result));
    }
}
