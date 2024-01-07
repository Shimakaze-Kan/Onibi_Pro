using System.ComponentModel.DataAnnotations;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Onibi_Pro.Communication.Models;

public class Message
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public List<MessageRecipient> Recipients { get; set; } = [];
    public DateTime SentTime { get; set; }
    public bool IsViewed { get; set; }

    [Required(ErrorMessage = "Text is required")]
    public string Text { get; set; } = null!;
}

public class MessageRecipient
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
}