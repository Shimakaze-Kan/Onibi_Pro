namespace Onibi_Pro.Application.Common.Models;
public class Notification
{
    public string Text { get; }
    public IReadOnlyCollection<RecipientStatus> Recipients { get; }

    public record RecipientStatus(Guid UserId);

    public Notification(string text, List<Guid> reciptients)
    {
        Text = text;
        Recipients = reciptients.ConvertAll(recipient => new RecipientStatus(recipient));
    }
};
