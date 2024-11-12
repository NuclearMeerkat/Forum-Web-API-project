namespace WebApp.Infrastructure.Entities;

public class MessageLike : BaseEntity
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public User User { get; set; }

    public Message Message { get; set; }

    public override object GetIdentifier()
    {
        return new ReportCompositeKey() { UserId = this.UserId, MessageId = this.MessageId };
    }
}
