namespace Onibi_Pro.Infrastructure.Email.Configurations;
internal sealed class ConfirmEmailConfiguration
{
    public const string Key = "ConfirmEmailConfiguration";

    public string SenderEmail { get; set; } = "";
    public string TemplateName { get; set; } = "";
    public string Subject { get; set; } = "";
    public string LogoPath { get; set; } = "";
}
