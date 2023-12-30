using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Packages.Commands.CreatePackage;
using Onibi_Pro.Application.Packages.Queries.GetPackageById;
using Onibi_Pro.Application.Packages.Queries.GetPackages;
using Onibi_Pro.Contracts.Shipments;
using Onibi_Pro.Contracts.Shipments.Common;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ShipmentsController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ShipmentsController(IMediator mediator,
        IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PackageItem), 200)]
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
    public async Task<IActionResult> CreatePackaget([FromBody] CreatePackageRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreatePackageCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(result => Ok(_mapper.Map<PackageItem>(result)), Problem);
    }

    [HttpGet("id/{packageId}")]
    [ProducesResponseType(typeof(PackageItem), 200)]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerOrManagerAccess)]
    public async Task<IActionResult> GetPackageById([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var query = new GetPackageByIdQuery(PackageId.Create(packageId));

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(response => Ok(_mapper.Map<PackageItem>(response)), Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetPackagesResponse), 200)]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerOrManagerAccess)]
    public async Task<IActionResult> GetAllPackages([FromQuery] GetPackagesRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetPackagesQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetPackagesResponse>(result));
    }
}
