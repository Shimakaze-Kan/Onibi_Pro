using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
using Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
using Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
using Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
using Onibi_Pro.Contracts.Restaurants;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
public class RestaurantsController : ApiBaseController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RestaurantsController(IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateRestaurantResponse), 200)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
    {
        var command = _mapper.Map<CreateRestaurantCommand>(request);

        var result = await _mediator.Send(command);

        return result.Match(result
            => Ok(_mapper.Map<CreateRestaurantResponse>(result)), Problem);
    }

    [HttpGet("{restaurantId}/employees")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetEmployeesResponse>), 200)]
    public async Task<IActionResult> GetEmployees([FromRoute] Guid restaurantId, [FromQuery] GetEmployeeRequest request)
    {
        var query = _mapper.Map<GetEmployeesQuery>((restaurantId, request));

        var result = await _mediator.Send(query);

        return result.Match(result
            => Ok(_mapper.Map<IReadOnlyCollection<GetEmployeesResponse>>(result)), Problem);
    }

    [HttpPost("{restaurantId}/employee")]
    [ProducesResponseType(typeof(CreateEmployeeResponse), 200)]
    public async Task<IActionResult> CreateEmployee([FromRoute] Guid restaurantId, [FromBody] CreateEmployeeRequest request)
    {
        var command = _mapper.Map<CreateEmployeeCommand>((restaurantId, request));

        var result = await _mediator.Send(command);

        return result.Match(result
            => Ok(_mapper.Map<CreateEmployeeResponse>(result)), Problem);
    }

    [HttpPut("{restaurantId}/employee")]
    [ProducesResponseType(typeof(CreateEmployeeResponse), 200)]
    public async Task<IActionResult> EditEmployee([FromRoute] Guid restaurantId, [FromBody] EditEmployeeRequest request)
    {
        var command = _mapper.Map<EditEmployeeCommand>((restaurantId, request));

        var result = await _mediator.Send(command);

        return result.Match(_ => Ok(), Problem);
    }
}
