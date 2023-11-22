using ErrorOr;
using MediatR;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Queries;
internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginQueryHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_authenticationService.Login(request.Email,
                                                                  request.Password));
    }
}
