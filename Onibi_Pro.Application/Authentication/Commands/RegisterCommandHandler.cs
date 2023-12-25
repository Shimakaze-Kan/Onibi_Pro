using ErrorOr;
using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<Success>>
{
    private readonly IRegisterService _registerService;
    private readonly ICurrentUserService _currentUserService;

    public RegisterCommandHandler(IRegisterService registerService,
        ICurrentUserService currentUserService)
    {
        _registerService = registerService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var newUserType = _currentUserService.UserType switch
        {
            UserTypes.GlobalManager => UserTypes.RegionalManager,
            UserTypes.RegionalManager => UserTypes.Manager,
            _ => throw new NotImplementedException()
        };

        return await _registerService.RegisterAsync(request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            newUserType,
            cancellationToken);
    }
}
