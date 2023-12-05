using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Menus.Commands.CreateMenu;
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
    public async Task<IActionResult> CreateMenu(CreateMenuRequest request)
    {
        var command = _mapper.Map<CreateMenuCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(result => Ok(_mapper.Map<CreateMenuResponse>(result)), Problem);
    }
}
