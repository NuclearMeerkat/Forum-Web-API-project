namespace WebApp.Core.Entities;

public class MessageLike : BaseEntity
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public User User { get; set; }

    public Message Message { get; set; }
}
