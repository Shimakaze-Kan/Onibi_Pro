using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Onibi_Pro.Application.Common.Interfaces.Authentication;
using Onibi_Pro.Application.Common.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Onibi_Pro.Infrastructure.Authentication;
internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtTokenSettings _jwtTokenSettings;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtTokenSettings> options)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtTokenSettings = options.Value;
    }

    public string GenerateToken(Guid userId, string firstName, string lastName)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenSettings.Secret)), SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            issuer: _jwtTokenSettings.Issuer,
            audience: _jwtTokenSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtTokenSettings.ExpirationTimeInMinutes),
            signingCredentials: signingCredentials,
            claims: claims);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
