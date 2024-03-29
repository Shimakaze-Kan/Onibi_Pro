﻿using Microsoft.AspNetCore.Mvc;

using Onibi_Pro.Communication.Common;
using Onibi_Pro.Communication.Models;
using Onibi_Pro.Communication.Repositories;

namespace Onibi_Pro.Communication.Controllers;
[Route("[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;

    public MessagesController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    [HttpGet("inbox")]
    public async Task<ActionResult<IReadOnlyCollection<MessageDto>>> GetInboxMessages(CancellationToken cancellationToken)
    {
        var userId = HeadersProvider.GetUserId(HttpContext);

        var result = await _messageRepository.GetReceivedMessagesAsync(userId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("outbox")]
    public async Task<ActionResult<IReadOnlyCollection<MessageDto>>> GetOutboxMessages(CancellationToken cancellationToken)
    {
        var userId = HeadersProvider.GetUserId(HttpContext);

        var result = await _messageRepository.GetSentMessagesAsync(userId, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{messageId}/markAsViewed")]
    public async Task<ActionResult> MarkMessageAsViewed(string messageId, CancellationToken cancellationToken)
    {
        var userId = HeadersProvider.GetUserId(HttpContext);

        await _messageRepository.MarkMessageAsViewedAsync(messageId, userId, cancellationToken);

        return Ok();
    }

    [HttpDelete("{messageId}/delete")]
    public async Task<ActionResult> MarkMessageAsDeleted(string messageId, CancellationToken cancellationToken)
    {
        var userId = HeadersProvider.GetUserId(HttpContext);

        await _messageRepository.DeleteMessageAsync(messageId, userId, cancellationToken);

        return Ok();
    }
}
