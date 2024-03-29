﻿using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<UserId>>
{
    private readonly IRegisterService _registerService;
    private readonly ICurrentUserService _currentUserService;

    public RegisterCommandHandler(IRegisterService registerService,
        ICurrentUserService currentUserService)
    {
        _registerService = registerService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<UserId>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _registerService.RegisterAsync(request.FirstName,
            request.LastName,
            request.Email,
            Passwords.InitialPassword,
            currentCreatorType: CreatorUserType.Create(_currentUserService.UserType),
            userType: request.UserType,
            cancellationToken: cancellationToken);
    }
}
