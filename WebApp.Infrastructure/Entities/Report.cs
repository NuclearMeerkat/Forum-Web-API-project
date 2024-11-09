using WebApp.Infrastructure.Enums;

namespace WebApp.Infrastructure.Entities;

public class Report : BaseEntity
{
    public int MessageId { get; set; }

    public int UserId { get; set; }

    public string Reason { get; set; }

    public ReportStatus Status { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ReviewedAt { get; set; }

    public Message Message { get; set; }

    public User User { get; set; }

    public override object GetIdentifier()
    {
        return new CompositeKey() { KeyPart1 = this.UserId, KeyPart2 = this.MessageId };
    }
}