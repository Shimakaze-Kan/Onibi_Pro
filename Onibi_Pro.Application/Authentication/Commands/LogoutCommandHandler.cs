using ErrorOr;
using MediatR;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Success>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUserService _currentUserService;

    public LogoutCommandHandler(IAuthenticationService authenticationService,
        ICurrentUserService currentUserService)
    {
        _authenticationService = authenticationService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var canGetUserInfo = _currentUserService.CanGetCurrentUser;

        if (!canGetUserInfo)
        {
            return Error.NotFound();
        }

        var userId = _currentUserService.UserId;

        _authenticationService.Logout(userId);
        await Task.CompletedTask;

        return new Success();
    }
}
