using Microsoft.AspNetCore.Mvc;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Contracts.Authentication;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ApiBaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var authResult = _authenticationService.Register(
            request.FirstName, request.LastName, request.Email, request.Password);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)), Problem);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(request.Email, request.Password);

        if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized, title: authResult.FirstError.Description);
        }

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)), Problem);
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.Id, authResult.FirstName, authResult.LastName, authResult.Email, authResult.Token);
    }
}
