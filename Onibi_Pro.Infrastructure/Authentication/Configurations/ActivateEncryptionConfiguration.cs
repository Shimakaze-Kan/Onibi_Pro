namespace Onibi_Pro.Infrastructure.Authentication.Configurations;
internal sealed class ActivateEncryptionConfiguration
{
    public const string Key = "GuidEncryptionConfiguration";

    public string EncryptionKey { get; set; } = "";
    public string InitializationVector { get; set; } = "";
    public int ExpirationInMinutes { get; set; }
}
