using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create(Notification notification)
    {
        await _notificationsRepository.Create(notification);
        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Notification notificationIn)
    {
        var notification = await _notificationsRepository.GetById(id);

        if (notification == null)
        {
            return NotFound();
        }

        await _notificationsRepository.Update(id, notificationIn);

        return NoContent();
    }

    [HttpGet("{startDateTime}/{endDateTime?}")]
    public async Task<ActionResult<List<Notification>>> GetChunk(DateTime startDateTime, DateTime? endDateTime = null)
    {
        var notifications = await _notificationsRepository.GetChunk(startDateTime, endDateTime);
        return notifications;
    }

    [HttpGet("{id:length(24)}", Name = "GetById")]
    public async Task<ActionResult<Notification>> GetById(string id)
    {
        var notification = await _notificationsRepository.GetById(id);

        if (notification == null)
        {
            return NotFound();
        }

        return notification;
    }
}