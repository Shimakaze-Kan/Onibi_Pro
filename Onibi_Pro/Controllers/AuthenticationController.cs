using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Authentication.Commands;
using Onibi_Pro.Application.Authentication.Queries;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Contracts.Authentication;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Controllers;

[Route("api/[controller]")]
public class AuthenticationController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AuthenticationController(IMediator mediator,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), 200)]
    [Authorize(Policy = AuthorizationPolicies.GlobalOrRegionalManagerAccess)]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        var authResult = await _mediator.Send(command, cancellationToken);

        return authResult.Match(result => Ok(result.Value), Problem);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<LoginQuery>(request);

        var authResult = await _mediator.Send(query, cancellationToken);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), Problem);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var logoutResult = await _mediator.Send(new LogoutCommand(), cancellationToken);

        return logoutResult.Match(
            logoutResult =>
            {
                HttpContext.Response.Cookies.Delete(AuthenticationKeys.CookieName);
                return Ok();
            }, Problem);
    }

    [AllowAnonymous]
    [HttpGet("isAuthenticated")]
    public ActionResult<bool> IsAuthenticated()
    {
        return Ok(_currentUserService.CanGetCurrentUser);
    }

    [AllowAnonymous]
    [HttpPut("ConfirmEmail/{*code}")]
    public async Task<IActionResult> ConfirmEmail([FromRoute] string code, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(code), cancellationToken);

        return result.Match(_ => Ok(), Problem);
    }
}
