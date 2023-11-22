using MediatR;
using Microsoft.AspNetCore.Mvc;
using Onibi_Pro.Application.Authentication.Commands;
using Onibi_Pro.Application.Authentication.Queries;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Contracts.Authentication;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ApiBaseController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName,
                                          request.LastName,
                                          request.Email,
                                          request.Password);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(MapAuthResult(authResult)), Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);

        var authResult = await _mediator.Send(query);

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
