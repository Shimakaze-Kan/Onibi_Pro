
using Microsoft.AspNetCore.SignalR;

using Onibi_Pro.Communication.Hubs;
using Onibi_Pro.Communication.Repositories;

namespace Onibi_Pro.Communication.BgWorkers;

public sealed class EventNotifier : BackgroundService
{
    private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);
    private readonly ILogger<EventNotifier> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly INotificationRepository _notificationRepository;

    public EventNotifier(ILogger<EventNotifier> logger,
        IHubContext<NotificationsHub> hubContext,
        INotificationRepository notificationRepository)
    {
        _logger = logger;
        _hubContext = hubContext;
        _notificationRepository = notificationRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            var dateTime = DateTime.UtcNow;

            _logger.LogInformation("Executing {Service} {Time}", nameof(EventNotifier), dateTime);

            var notifications = await _notificationRepository.GetChunk(DateTime.UtcNow.AddDays(-1));

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
                        await _hubContext.Clients.Group(recipient.UserId.ToString()).SendAsync("ReceiveNotification", notification.Text);
                    }

                    _logger.LogInformation("Executing {Service} {Time}: sent message '{message}'", nameof(EventNotifier), dateTime, notification.Text);

                    notification.IsRead = true;
                    await _notificationRepository.Update(notification.Id, notification);
                }
            }
        }
    }

    private async Task SendMessageToGroup(string groupName, string message)
    {
        await _hubContext.Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }
}
