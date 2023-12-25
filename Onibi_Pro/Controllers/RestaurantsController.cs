using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Restaurants.Commands.AssignManager;
using Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
using Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
using Onibi_Pro.Application.Restaurants.Commands.CreateSchedule;
using Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
using Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
using Onibi_Pro.Application.Restaurants.Queries.GetSchedules;
using Onibi_Pro.Contracts.Restaurants;
using Onibi_Pro.Shared;

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
    [Authorize(Policy = AuthorizationPolicies.GlobalManagerAccess)]
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
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
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
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
    public async Task<IActionResult> EditEmployee([FromRoute] Guid restaurantId, [FromBody] EditEmployeeRequest request)
    {
        var command = _mapper.Map<EditEmployeeCommand>((restaurantId, request));

        var result = await _mediator.Send(command);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPost("{restaurantId}/manager")]
    [Authorize(Policy = AuthorizationPolicies.GlobalManagerAccess)]
    public async Task<IActionResult> AssignManager([FromRoute] Guid restaurantId, [FromBody] AssignManagerRequest request)
    {
        var command = _mapper.Map<AssignManagerCommand>((restaurantId, request));

        var result = await _mediator.Send(command);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPost("{restaurantId}/schedule")]
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
    public async Task<IActionResult> CreateSchedule([FromRoute] Guid restaurantId, [FromBody] CreateScheduleRequest request)
    {
        var command = _mapper.Map<CreateScheduleCommand>((restaurantId, request));

        var result = await _mediator.Send(command);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpGet("{restaurantId}/schedule")]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetScheduleResponse>), 200)]
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
    public async Task<IActionResult> GetSchedules([FromRoute] Guid restaurantId)
    {
        var result = await _mediator.Send(new GetScheduleQuery(restaurantId));

        return result.Match(response 
            => Ok(_mapper.Map<IReadOnlyCollection<GetScheduleResponse>>(response)), Problem);
    }
}
