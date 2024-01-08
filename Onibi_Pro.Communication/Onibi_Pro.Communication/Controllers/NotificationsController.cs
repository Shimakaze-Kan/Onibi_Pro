using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Communication.Common;
using Onibi_Pro.Communication.Models;
using Onibi_Pro.Communication.Repositories;

namespace Onibi_Pro.Communication.Controllers;
[ApiController]
[Route("[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationRepository _notificationsRepository;

    public NotificationsController(INotificationRepository notificationsService)
    {
        _notificationsRepository = notificationsService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Notification notification, CancellationToken cancellationToken)
    {
        await _notificationsRepository.CreateAsync(notification, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Notification notificationIn, CancellationToken cancellationToken)
    {
        var notification = await _notificationsRepository.GetByIdAsync(id, cancellationToken);

        if (notification == null)
        {
            return NotFound();
        }

        await _notificationsRepository.UpdateAsync(id, notificationIn, cancellationToken);

        return NoContent();
    }

    [HttpGet("{startDateTime}/{endDateTime?}")]
    public async Task<ActionResult<List<Notification>>> GetChunk(DateTime startDateTime, 
        DateTime? endDateTime = null, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationsRepository.GetChunkAsync(startDateTime, endDateTime, cancellationToken);
        return notifications;
    }

    [HttpGet("{id:length(24)}", Name = "GetById")]
    public async Task<ActionResult<Notification>> GetById(string id, CancellationToken cancellationToken)
    {
        var notification = await _notificationsRepository.GetByIdAsync(id, cancellationToken);

        if (notification == null)
        {
            return NotFound();
        }

        return notification;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDto>>> GetAllNotificationsForUser(CancellationToken cancellationToken)
    {
        var userId = HeadersProvider.GetUserId(HttpContext);

        return await _notificationsRepository.GetAllForUserAsync(userId, cancellationToken);
    }

    [HttpDelete("{notificationIds}")]
    public async Task<ActionResult> MarkNotificationAsDeleted(string notificationIds, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(notificationIds?.Trim()))
        {
            return BadRequest();
        }

        var ids = notificationIds.Split(',').Select(id => id.Trim()).ToList();

        foreach (var id in ids)
        {
            if (id.Length != 24)
            {
                return BadRequest(id);
            }
        }

        var userId = HeadersProvider.GetUserId(HttpContext);

        await _notificationsRepository.MarkAsDeletedAsync(ids, userId, cancellationToken);

        return Ok();
    }
}