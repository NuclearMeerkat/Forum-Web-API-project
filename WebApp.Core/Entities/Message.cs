namespace WebApp.Core.Entities;

public class Message : BaseEntity
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsEdited { get; set; }

    public int LikesCounter { get; set; }

    public DateTime CreatedAt { get; set; }

    public int TopicId { get; set; }

    public Topic Topic { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public int? ParentMessageId { get; set; }

    public Message? ParentMessage { get; set; }

    public ICollection<Message> Replies { get; set; } = new List<Message>();

    public ICollection<MessageLike> Likes { get; set; } = new List<MessageLike>();

    public ICollection<Report> Reports { get; set; } = new List<Report>();
}
