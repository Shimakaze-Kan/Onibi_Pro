using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IAuthenticationService _authenticationService;

    public RegisterCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _authenticationService.RegisterAsync(request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            cancellationToken);
    }
}
