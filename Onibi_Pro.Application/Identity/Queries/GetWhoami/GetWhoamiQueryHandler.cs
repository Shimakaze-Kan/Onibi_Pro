using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Identity.Queries.GetWhoami;
internal sealed class GetWhoamiQueryHandler(ICurrentUserService currentUserService)
    : IRequestHandler<GetWhoamiQuery, ErrorOr<WhoamiDto>>
{
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<ErrorOr<WhoamiDto>> Handle(GetWhoamiQuery request, CancellationToken cancellationToken)
    {
        var userId = UserId.Create(_currentUserService.UserId);
        var email = _currentUserService.Email;
        var firstName = _currentUserService.FirstName;
        var lastName = _currentUserService.LastName;
        var userType = _currentUserService.UserType;

        return await Task.FromResult(new WhoamiDto(userId, email, firstName, lastName, userType));
    }
}
