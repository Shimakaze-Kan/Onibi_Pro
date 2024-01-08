namespace Onibi_Pro.Communication.Models;

public class MessageDto
{
    public string Id { get; }
    public string Title { get; }
    public string AuthorName { get; }
    public Guid AuthorId { get; }
    public List<MessageRecipientDto> Recipients { get; set; } = [];
    public DateTime SentTime { get; }
    public string Text { get; }
    public bool IsViewed { get; }

    public MessageDto(Message message, bool isViewed)
    {
        Id = message.Id;
        Title = message.Title;
        AuthorName = message.AuthorName;
        AuthorId = message.AuthorId;
        Recipients = message.Recipients.ConvertAll(
            recipient => new MessageRecipientDto(recipient));
        SentTime = message.SentTime;
        Text = message.Text;
        IsViewed = isViewed;
    }
}

public class MessageRecipientDto
{
    public Guid UserId { get; }
    public string Name { get; }

    public MessageRecipientDto(MessageRecipient messageRecipient)
    {
        UserId = messageRecipient.UserId;
        Name = messageRecipient.Name;
    }
}