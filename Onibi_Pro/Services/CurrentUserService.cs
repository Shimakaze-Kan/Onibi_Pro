using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Shared;
using System.IdentityModel.Tokens.Jwt;

namespace Onibi_Pro.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private JwtSecurityToken _jwtToken = null!;

    public string FirstName
        => _jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.GivenName).Value;

    public string LastName
        => _jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.FamilyName).Value;
    
    public string Email
        => _jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;

    public Guid UserId
    {
        get
        {
            var userId = _jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            return Guid.Parse(userId);
        }
    }

    public bool CanGetCurrentUser => _jwtToken is not null;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return;
        }

        var token = httpContextAccessor.HttpContext.GetToken();

        if (string.IsNullOrEmpty(token))
        {
            return;
        }

        ReadToken(token);
    }

    private void ReadToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            return;
        }

        if (handler.ReadToken(token) is not JwtSecurityToken tokenObject)
        {
            return;
        }

        _jwtToken = tokenObject;
    }
}
