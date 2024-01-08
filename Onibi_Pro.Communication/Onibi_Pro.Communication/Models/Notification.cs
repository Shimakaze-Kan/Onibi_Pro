using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Onibi_Pro.Communication.Models;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required(ErrorMessage = "Text is required")]
    public string Text { get; set; } = null!;

    public List<RecipientStatus> Recipients { get; set; } = [];

    public bool IsRead { get; set; } = false;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}

public class RecipientStatus
{
    public Guid UserId { get; set; }

    public bool IsDeleted { get; set; } = false;
}