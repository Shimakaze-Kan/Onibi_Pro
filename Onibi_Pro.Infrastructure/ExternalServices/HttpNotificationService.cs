using System.Text;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Infrastructure.ExternalServices.Configurations;
using Onibi_Pro.Infrastructure.ExternalServices.Exceptions;

namespace Onibi_Pro.Infrastructure.ExternalServices;
internal sealed class HttpNotificationService : INotificationService
{
    private const int RetryCount = 3;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CommunicationServiceConfiguration _options;
    private readonly ILogger<HttpNotificationService> _logger;

    public HttpNotificationService(IHttpClientFactory httpClientFactory,
        IOptions<CommunicationServiceConfiguration> options,
        ILogger<HttpNotificationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendNotification(Notification notification, CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(HttpNotificationService));
        httpClient.BaseAddress = new Uri(_options.BaseUrl);
        var content = new StringContent(JsonConvert.SerializeObject(notification), Encoding.UTF8, "application/json");

        for (int i = 0; i < RetryCount; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Notification sending was canceled after attempt: {tryNumber}", i);
                return;
            }

            try
            {
                var response = await httpClient.PostAsync(_options.SendNotificationUrl, content, cancellationToken);

                response.EnsureSuccessStatusCode();

                _logger.LogInformation("Notification sent successfully on attempt: {tryNumber}", i);

                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send notification on attempt {tryNumber}", i);
                await Task.Delay(500, cancellationToken);
            }
        }

        throw new RequestFailedException();
    }
}
