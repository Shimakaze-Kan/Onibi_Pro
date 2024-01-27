using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Statistics.GetIngredientStatistics;
using Onibi_Pro.Application.Statistics.GetTopMenuItems;
using Onibi_Pro.Contracts.Statistics;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StatisticsController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("IngredientStatistics")]
    [Authorize(Policy = AuthorizationPolicies.GlobalManagerAccess)]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetIngredientStatisticsResponse>), 200)]
    public async Task<IActionResult> GetIngredientStatistics(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetIngredientStatisticsQuery(), cancellationToken);

        return result.Match(result
            => Ok(_mapper.Map<IReadOnlyCollection<GetIngredientStatisticsResponse>>(result)), Problem);
    }
    
    [HttpGet("TopMenuItems")]
    [Authorize(Policy = AuthorizationPolicies.GlobalManagerAccess)]
    [ProducesResponseType(typeof(IReadOnlyCollection<GetTopMenuItemsResponse>), 200)]
    public async Task<IActionResult> GetTopMenuItems(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTopMenuItemsQuery(), cancellationToken);

        return result.Match(result
            => Ok(_mapper.Map<IReadOnlyCollection<GetTopMenuItemsResponse>>(result)), Problem);
    }
}
