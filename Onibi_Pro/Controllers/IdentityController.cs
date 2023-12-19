using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
using Onibi_Pro.Contracts.Menus;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
public class IdentityController : ApiBaseController
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("managerDetails/{managerId}")]
    [ProducesResponseType(typeof(ManagerDetailsDto), 200)]
    public async Task<IActionResult> GetManagerDetails([FromRoute] Guid managerId)
    {
        var query = new GetManagerDetailsQuery(UserId.Create(managerId));

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
