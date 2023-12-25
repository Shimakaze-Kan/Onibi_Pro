using ErrorOr;
using MediatR;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Queries;
internal sealed class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly ILoginService _loginService;

    public LoginQueryHandler(ILoginService loginService)
    {
        _loginService = loginService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        return await _loginService.LoginAsync(request.Email, request.Password, cancellationToken);
    }
}
