using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Menus.Commands.CreateMenu;
using Onibi_Pro.Application.Menus.Queries.GetMenus;
using Onibi_Pro.Contracts.Menus;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
public class MenusController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public MenusController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateMenuResponse), 200)]
    public async Task<IActionResult> CreateMenu(CreateMenuRequest request)
    {
        var command = _mapper.Map<CreateMenuCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(result => Ok(_mapper.Map<CreateMenuResponse>(result)), Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetMenusResponse>), 200)]
    public async Task<IActionResult> GetMenus()
    {
        var result = await _mediator.Send(new GetMenusQuery());

        return result.Match(result => Ok(_mapper.Map<IReadOnlyCollection<GetMenusResponse>>(result)), Problem);
    }
}
