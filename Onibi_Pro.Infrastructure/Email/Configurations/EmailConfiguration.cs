namespace Onibi_Pro.Infrastructure.Email.Configurations;
internal sealed class EmailConfiguration
{
    public const string Key = "EmailConfig";

    public string Host { get; set; } = "";
    public int Port { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
