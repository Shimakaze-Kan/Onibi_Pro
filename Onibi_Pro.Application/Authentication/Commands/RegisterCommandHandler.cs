using ErrorOr;
using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUserService _currentUserService;

    public RegisterCommandHandler(IAuthenticationService authenticationService,
        ICurrentUserService currentUserService)
    {
        _authenticationService = authenticationService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUserType = _currentUserService.UserType switch
        {
            UserTypes.GlobalManager => UserTypes.RegionalManager,
            UserTypes.RegionalManager => UserTypes.Manager,
            _ => throw new NotImplementedException()
        };

        return await _authenticationService.RegisterAsync(request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            newUserType,
            cancellationToken);
    }
}
