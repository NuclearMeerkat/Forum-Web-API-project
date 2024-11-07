namespace WebApp.Core.Models.MessageModels;

public class MessageCreateModel
{
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ParentMessageId { get; set; }
}
