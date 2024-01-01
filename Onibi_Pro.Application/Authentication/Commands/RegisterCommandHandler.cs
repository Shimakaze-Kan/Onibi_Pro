using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

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
        return await _registerService.RegisterAsync(request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            currentCreatorType: CreatorUserType.Create(_currentUserService.UserType),
            cancellationToken);
    }
}
