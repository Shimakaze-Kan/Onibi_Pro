using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Packages.Commands.CreatePackage;
using Onibi_Pro.Application.Packages.Queries.GetPackageById;
using Onibi_Pro.Contracts.Packages;
using Onibi_Pro.Domain.PackageAggregate.ValueObjects;

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
    public async Task<IActionResult> CreatePackaget([FromBody] CreatePackageRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreatePackageCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result); //zmapować na coś
    }

    [HttpGet("{packageId}")]
    public async Task<IActionResult> GetPackageById([FromRoute] Guid packageId, CancellationToken cancellationToken)
    {
        var query = new GetPackageByIdQuery(PackageId.Create(packageId));

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result.Value);
    }
}
