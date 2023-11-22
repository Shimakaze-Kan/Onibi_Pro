namespace Onibi_Pro.Infrastructure.Authentication;
public class JwtTokenSettings
{
    public const string SectionName = "JwtSettings";

    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpirationTimeInMinutes { get; init; }
}
