namespace WebApp.Core.Entities;

public class MessageLike : BaseEntity
{
    public int UserId { get; set; }

    public int MessageId { get; set; }

    public User User { get; set; }

    public Message Message { get; set; }

    public override object GetIdentifier()
    {
        return new CompositeKey() { KeyPart1 = this.UserId, KeyPart2 = this.MessageId };
    }
}
