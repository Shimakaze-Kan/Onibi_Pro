using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Identity.Queries.GetManagerDetails;
internal sealed class GetManagerDetailsQueryHandler : IRequestHandler<GetManagerDetailsQuery, ErrorOr<ManagerDetailsDto>>
{
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public GetManagerDetailsQueryHandler(IManagerDetailsService managerDetailsService,
        ICurrentUserService currentUserService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _managerDetailsService = managerDetailsService;
        _currentUserService = currentUserService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<ErrorOr<ManagerDetailsDto>> Handle(GetManagerDetailsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserType == UserTypes.Manager &&
            _currentUserService.UserId != request.UserId.Value)
        {
            return Error.Unauthorized();
        }

        if (_currentUserService.UserType == UserTypes.RegionalManager)
        {
            var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

            var isDependentManager = regionalManagerDetails.ManagerIds.Any(id => id == request.UserId.Value);

            if (!isDependentManager)
            {
                return Error.Unauthorized();
            }
        }

        return await _managerDetailsService.GetManagerDetailsAsync(request.UserId);
    }
}
