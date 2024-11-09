namespace WebApp.Infrastructure.Models.MessageModels;

public class MessageBriefModel
{
    public int Id { get; set; }

    public string SenderNickname { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public int TopicId { get; set; }
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public int RepliesCount { get; set; }

    public int? ParentMessageId { get; set; }

    public int LikesCounter { get; set; }
}
