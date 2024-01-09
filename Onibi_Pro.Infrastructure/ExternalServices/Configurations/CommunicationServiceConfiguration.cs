namespace Onibi_Pro.Infrastructure.ExternalServices.Configurations;
internal sealed class CommunicationServiceConfiguration
{
    public const string Key = "Communication";

    public string BaseUrl { get; set; } = "";
    public string SendNotificationUrl { get; set; } = "";
}
