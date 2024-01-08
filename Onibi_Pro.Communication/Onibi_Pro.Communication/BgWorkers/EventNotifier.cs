
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

using Onibi_Pro.Communication.Hubs;
using Onibi_Pro.Communication.Models;
using Onibi_Pro.Communication.Repositories;

namespace Onibi_Pro.Communication.BgWorkers;

public sealed class EventNotifier : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
    private readonly ILogger<EventNotifier> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly INotificationRepository _notificationRepository;
    private readonly IOptions<NotificationBgWorkerConfig> _options;

    public EventNotifier(ILogger<EventNotifier> logger,
        IHubContext<NotificationsHub> hubContext,
        INotificationRepository notificationRepository,
        IOptions<NotificationBgWorkerConfig> options)
    {
        _logger = logger;
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
        _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options?.Value.Enabled == false)
        {
            _logger.LogInformation("{Service} has been disabled, {Time}", nameof(EventNotifier), DateTime.UtcNow);
            return;
        }

        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.UtcNow;

            _logger.LogInformation("Executing {Service} {Time}", nameof(EventNotifier), dateTime);

            var notifications = await _notificationRepository.GetChunkAsync(DateTime.UtcNow.AddDays(-1));

            if (notifications == null)
            {
                _logger.LogInformation("Executing {Service} {Time}: no notifications", nameof(EventNotifier), dateTime);
            }
            else
            {
                foreach (var notification in notifications)
                {
                    foreach (var recipient in notification.Recipients)
                    {
                        NotificationDto data = new(notification.Id, notification.Text, notification.SentAt);
                        await _hubContext.Clients.Group(recipient.UserId.ToString()).SendAsync("ReceiveNotification", data);
                    }

                    _logger.LogInformation("Executing {Service} {Time}: sent message '{message}'", nameof(EventNotifier), dateTime, notification.Text);

                    notification.IsRead = true;
                    await _notificationRepository.UpdateAsync(notification.Id, notification);
                }
            }
        }
    }

    private async Task SendMessageToGroup(string groupName, string message)
    {
        await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }
}
