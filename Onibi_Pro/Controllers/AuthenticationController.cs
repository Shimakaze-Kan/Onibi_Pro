using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Application.Authentication.Commands;
using Onibi_Pro.Application.Authentication.Queries;
using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Contracts.Authentication;
using Onibi_Pro.Contracts.Identity;
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
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);

        var authResult = await _mediator.Send(command);

        return authResult.Match(result => Ok(result.Value), Problem);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        var authResult = await _mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), Problem);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var logoutResult = await _mediator.Send(new LogoutCommand());

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
}
