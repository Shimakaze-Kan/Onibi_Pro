using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Packages.Commands.AcceptPackage;
using Onibi_Pro.Application.Packages.Commands.ConfirmDelivery;
using Onibi_Pro.Application.Packages.Commands.CreatePackage;
using Onibi_Pro.Application.Packages.Commands.PickUpPackage;
using Onibi_Pro.Application.Packages.Commands.RejectPackage;
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
    [Authorize(Policy = AuthorizationPolicies.RegionalOrCourierOrManagerAccess)]
    public async Task<IActionResult> GetPackageById([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var query = new GetPackageByIdQuery(PackageId.Create(packageId));

        var result = await _mediator.Send(query, cancellationToken);

        return result.Match(response => Ok(_mapper.Map<PackageItem>(response)), Problem);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetPackagesResponse), 200)]
    [Authorize(Policy = AuthorizationPolicies.RegionalOrCourierOrManagerAccess)]
    public async Task<IActionResult> GetAllPackages([FromQuery] GetPackagesRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetPackagesQuery>(request);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetPackagesResponse>(result));
    }

    [HttpPut("approveShipment/{packageId}")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerAccess)]
    public async Task<IActionResult> ApprovePackage([FromRoute] Guid packageId,
        [FromBody] AcceptPackageRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AcceptPackageCommand>((packageId, request));

        var result = await _mediator.Send(command, cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPut("rejectShipment/{packageId}")]
    [Authorize(Policy = AuthorizationPolicies.RegionalManagerOrManagerAccess)]
    public async Task<IActionResult> RejectPackage([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RejectPackageCommand(PackageId.Create(packageId)), cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPut("pickupShipment/{packageId}")]
    [Authorize(Policy = AuthorizationPolicies.CourierAccess)]
    public async Task<IActionResult> PickUpPackage([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PickUpPackageCommand(PackageId.Create(packageId)), cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }

    [HttpPut("confirmDelivery/{packageId}")]
    [Authorize(Policy = AuthorizationPolicies.ManagerAccess)]
    public async Task<IActionResult> ConfirmPackageDelivery([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfirmDeliveryCommand(PackageId.Create(packageId)), cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }
}
