using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Identity.Queries.GetRegionalManagerDetails;
internal sealed class GetRegionalManagerDetailsQueryHandler : IRequestHandler<GetRegionalManagerDetailsQuery, ErrorOr<RegionalManagerDetailsDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public GetRegionalManagerDetailsQueryHandler(ICurrentUserService currentUserService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _currentUserService = currentUserService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<ErrorOr<RegionalManagerDetailsDto>> Handle(GetRegionalManagerDetailsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserType == UserTypes.RegionalManager &&
            _currentUserService.UserId != request.UserId.Value)
        {
            return Error.Unauthorized();
        }

        return await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(request.UserId);
    }
}
