﻿using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Services.Authentication;

namespace Onibi_Pro.Application.Authentication.Commands;
internal sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Success>>
{
    private readonly ILoginService _loginService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(ILoginService loginService,
        ICurrentUserService currentUserService,
        ILogger<LogoutCommandHandler> logger)
    {
        _loginService = loginService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var canGetUserInfo = _currentUserService.CanGetCurrentUser;

        if (!canGetUserInfo)
        {
            _logger.LogWarning("No user found to log out.");
            return Error.NotFound();
        }

        var userId = _currentUserService.UserId;

        await _loginService.LogoutAsync(userId, cancellationToken);
        await Task.CompletedTask;

        return new Success();
    }
}
