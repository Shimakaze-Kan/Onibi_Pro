namespace Onibi_Pro.Communication.Models;

public sealed class CommunicationDatabaseSettings
{
    public const string Key = "CommunicationDatabaseSettings";

    public string NotificationsCollectionName { get; set; } = null!;
    public string MessagesCollectionName { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
