namespace WebApp.Core.Models;

public class MessageModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TopicId { get; set; }

    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int LikeCount { get; set; }

    public int? ParentMessageId { get; set; }

    public MessageModel? ParentMessage { get; set; }

    public ICollection<MessageModel> Replies { get; set; } = new List<MessageModel>();

    public ICollection<ReportModel> Reports { get; set; } = new List<ReportModel>();
}
